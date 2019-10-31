using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RetireNet.Packages.Tool.Extensions;
using RetireNet.Packages.Tool.Models;
using RetireNet.Packages.Tool.Services.DotNet;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireLogger
    {
        private readonly ILogger<RetireLogger> _logger;
        private readonly RetireApiClient _retireApiClient;
        private readonly IAssetsFileParser _nugetreferenceservice;
        private readonly UsagesFinder _usageFinder;
        private readonly DotNetRestorer _restorer;
        private readonly IFileService _fileService;

        public RetireLogger(ILogger<RetireLogger> logger, RetireApiClient retireApiClient, IAssetsFileParser nugetreferenceservice, UsagesFinder usageFinder, DotNetRestorer restorer, IFileService fileService)
        {
            _logger = logger;
            _retireApiClient = retireApiClient;
            _nugetreferenceservice = nugetreferenceservice;
            _usageFinder = usageFinder;
            _restorer = restorer;
            _fileService = fileService;
        }

        public void LogPackagesToRetire()
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
                return;
            }

            var lockFiles = _fileService.ReadLockFiles();
            if (!lockFiles.Any())
            {
                _logger.LogError("No assets found. Are you running the tool from a folder missing a csproj or sln?");
                return;
            }

            foreach (var lockFile in lockFiles)
            {
                _logger.LogInformation($"Analyzing '{lockFile.PackageSpec.Name}'".Green());

                List<NugetReference> nugetReferences;
                try
                {
                    nugetReferences = _nugetreferenceservice.GetNugetReferences(lockFile).ToList();
                }
                catch (NoAssetsFoundException)
                {
                    _logger.LogError("No assets found. Are you running the tool from a folder missing a csproj?");
                    return;
                }

                _logger.LogDebug($"Found in total {nugetReferences.Count} references of NuGets (direct & transient)");

                var usages = _usageFinder.FindUsagesOf(nugetReferences, packagesToRetire);

                if (usages.Any())
                {
                    var plural = usages.Count > 1 ? "s" : string.Empty;
                    var grouped = usages.GroupBy(g => g.NugetReference.ToString());
                    var sb = new StringBuilder();
                    sb.AppendLine($"Found use of {grouped.Count()} vulnerable libs in {usages.Count} dependency path{plural}.");

                    foreach (var group in grouped)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"* {group.FirstOrDefault().Description} in {group.Key.Red()}");
                        sb.AppendLine(group.FirstOrDefault().IssueUrl.ToString());

                        if (_logger.IsEnabled(LogLevel.Debug))
                        {
                            foreach (var usage in group.Where(x => !x.IsDirect))
                            {
                                sb.AppendLine(usage.ReadPath());
                            }
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
        }
    }
}
