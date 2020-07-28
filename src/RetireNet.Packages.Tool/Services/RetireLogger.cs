using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RetireNet.Packages.Tool.Extensions;
using RetireNet.Packages.Tool.Models;
using RetireNet.Packages.Tool.Models.Report;
using RetireNet.Packages.Tool.Services.DotNet;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireLogger
    {
        private readonly ILogger<RetireLogger> _logger;
        private readonly RetireApiClient _retireApiClient;
        private readonly IAssetsFileParser _nugetReferenceService;
        private readonly UsagesFinder _usageFinder;
        private readonly DotNetRestorer _restorer;
        private readonly IFileService _fileService;
        private readonly IExitCodeHandler _exitCodeHandler;
        private Report _report;

        public RetireLogger(ILogger<RetireLogger> logger, RetireApiClient retireApiClient, IAssetsFileParser nugetReferenceService,
            UsagesFinder usageFinder, DotNetRestorer restorer, IFileService fileService, IExitCodeHandler exitCodeHandler)
        {
            _logger = logger;
            _retireApiClient = retireApiClient;
            _nugetReferenceService = nugetReferenceService;
            _usageFinder = usageFinder;
            _restorer = restorer;
            _fileService = fileService;
            _exitCodeHandler = exitCodeHandler;
        }

        public Report LogPackagesToRetire()
        {
            _report = new Report();

            try
            {
                LogPackagesToRetireInternal();
                return _report;
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                _exitCodeHandler.HandleExitCode(ExitCode.DIRECTORY_NOT_FOUND, true);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                _exitCodeHandler.HandleExitCode(ExitCode.FILE_NOT_FOUND, true);
            }
            catch (NoAssetsFoundException)
            {
                _logger.LogError("No assets found. Are you running the tool from a folder missing a csproj?");
                _exitCodeHandler.HandleExitCode(ExitCode.ASSETS_NOT_FOUND, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _exitCodeHandler.HandleExitCode(ExitCode.UNEXPECTED_TERMINATION, true);
            }

            return null;
        }

        internal void LogPackagesToRetireInternal()
        {
            // removing this line breaks logging somehow.
            _logger.LogInformation("Scan starting".Green());

            var packagesToRetire = _retireApiClient.GetPackagesToRetire().ToList();
            foreach (var p in packagesToRetire)
            {
                foreach (var package in p.Packages)
                {
                    _logger.LogTrace($"Looking for {package.Id}/{package.Affected}".Orange());
                }
            }

            var status = _restorer.Restore();
            if (status.IsSuccess)
            {
                _logger.LogDebug("`dotnet restore:`" + status.Output);
            }
            else
            {
                _logger.LogDebug("`dotnet restore output:`" + status.Output);
                _logger.LogDebug("`dotnet restore errors:`" + status.Errors);
                _logger.LogDebug("`dotnet restore exitcode:`" + status.ExitCode);

                _logger.LogError("Failed to `dotnet restore`. Is the current dir missing a csproj?");
                _exitCodeHandler.HandleExitCode(status.ExitCode, true);
            }

            var lockFiles = _fileService.ReadLockFiles();
            var foundVulnerabilities = false;
            foreach (var lockFile in lockFiles)
            {
                _logger.LogInformation($"Analyzing '{lockFile.PackageSpec.Name}'".Green());

                var nugetReferences = _nugetReferenceService.GetNugetReferences(lockFile).ToList();
                _logger.LogDebug($"Found in total {nugetReferences.Count} references of NuGets (direct & transient)");

                var usages = _usageFinder.FindUsagesOf(nugetReferences, packagesToRetire);
                if (usages.Any())
                {
                    foundVulnerabilities = true;

                    var project = new Project
                    {
                        Name = lockFile.PackageSpec.Name,
                        Path = lockFile.PackageSpec.FilePath,
                    };
                    _report.Add(project);

                    var plural = usages.Count > 1 ? "s" : string.Empty;
                    var grouped = usages.GroupBy(g => g.NugetReference.ToString());
                    var sb = new StringBuilder();
                    sb.AppendLine($"Found use of {grouped.Count()} vulnerable libs in {usages.Count} dependency path{plural}.");

                    foreach (var group in grouped)
                    {
                        var firstGroup = group.FirstOrDefault();
                        var keyPieces = group.Key.Split("/");
                        var issue = new PackageIssue
                        {
                            Name = keyPieces[0],
                            Version = keyPieces[1],
                            Description = firstGroup?.Description,
                            IssueUrl = firstGroup?.IssueUrl.ToString(),
                            ProblemPackage = firstGroup?.GetPackageChain().LastOrDefault(),
                            PackageChains = group.Select(g => g.GetPackageChain()).ToList(),
                        };
                        project.Issues.Add(issue);
                        sb.AppendLine();
                        sb.AppendLine($"* {firstGroup?.Description} in {group.Key.Red()}");
                        sb.AppendLine(firstGroup?.IssueUrl.ToString());

                        foreach (var usage in group.Where(x => !x.IsDirect))
                        {
                            sb.AppendLine(usage.ReadPath());
                        }
                    }

                    sb.AppendLine();
                    _logger.LogError(sb.ToString());
                }
                else
                {
                    _logger.LogInformation("Found no usages of vulnerable libs!".Green());
                }
            }

            _logger.LogInformation("Scan complete.");

            if (foundVulnerabilities)
            {
                _exitCodeHandler.HandleExitCode(ExitCode.FOUND_VULNERABILITIES);
            }
        }
    }
}
