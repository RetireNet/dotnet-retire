using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.ProjectModel;
using RetireNet.Packages.Tool.Extensions;

namespace RetireNet.Packages.Tool.Services
{
    public class FileService : IFileService
    {
        private const string _objFolderName = "obj";
        private const string _assetsFileName = "project.assets.json";
        private readonly ILogger<FileService> _logger;
        private readonly IOptions<RetireServiceOptions> _options;

        public FileService(ILogger<FileService> logger, IOptions<RetireServiceOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public IEnumerable<LockFile> ReadLockFiles()
        {
            var lockfiles = new List<LockFile>(1);
            try
            {
                var file = GetFile();
                if (Path.GetExtension(file).Equals(".sln", StringComparison.OrdinalIgnoreCase))
                {
                    var assetsFiles = GetAssetFilesFromSolution(file);
                    var plural = assetsFiles.Count() > 1 ? "s" : string.Empty;
                    _logger.LogDebug($"Found solution {Path.GetFileName(file)} with {assetsFiles.Count()} project{plural} in it".Green());
                    foreach (var assetsFile in assetsFiles)
                    {
                        _logger.LogDebug($"Found {_assetsFileName} file at '{assetsFile}'".Green());
                        lockfiles.Add(LockFileUtilities.GetLockFile(assetsFile, new NuGetLogger(_logger)));
                    }
                }
                else
                {
                    var assetsFile = Directory.EnumerateFiles(PathOfAssetsFile(), _assetsFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (assetsFile != null)
                    {
                        _logger.LogDebug($"Found {_assetsFileName} file at '{assetsFile}'".Green());
                        lockfiles.Add(LockFileUtilities.GetLockFile(assetsFile, new NuGetLogger(_logger)));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Could not find {_assetsFileName} file at '{PathOfAssetsFile()}'");
                _logger.LogDebug(e.ToString());
            }

            if (lockfiles.Count > 0)
            {
                return lockfiles;
            }

            throw new NoAssetsFoundException();
        }

        public string PathOfAssetsFile()
        {
            return Path.Combine(GetDirectory(), _objFolderName);
        }

        public string GetDirectory()
        {
            if (_options.Value.Path != null)
            {
                if (File.Exists(_options.Value.Path))
                {
                    return Path.GetDirectoryName(_options.Value.Path);
                }

                if (Directory.Exists(_options.Value.Path))
                {
                    return _options.Value.Path;
                }

                throw new DirectoryNotFoundException($"The path '{_options.Value.Path}' is neither a file or directory");
            }

            return Directory.GetCurrentDirectory();
        }

        public string GetFile()
        {
            var path = _options.Value.Path ?? Directory.GetCurrentDirectory();

            // Path is a file
            if (File.Exists(path))
            {
                return path;
            }

            // Path is a directory
            if (Directory.Exists(path))
            {   // Search for solution
                var candidates = Directory.EnumerateFiles(path, "*.sln", SearchOption.TopDirectoryOnly);
                if (!candidates.Any())
                {   // Fallback: Search for asset file
                    candidates = Directory.EnumerateFiles(Path.Combine(path, _objFolderName), _assetsFileName, SearchOption.TopDirectoryOnly);
                }

                if (candidates.Count() == 1)
                {
                    return candidates.First();
                }
            }

            throw new FileNotFoundException($"No or multiple .sln or assets found in path '{path}'");
        }

        public static IEnumerable<string> GetAssetFilesFromSolution(string solutionFile)
        {
            var content = File.ReadAllText(solutionFile);
            var projReg = new Regex("Project\\(\"\\{[\\w-]*\\}\"\\) = \"([\\w _]*.*)\", \"(.*\\.(cs|vcx|vb)proj)\"", RegexOptions.Compiled);
            var matches = projReg.Matches(content).Cast<Match>();

            if (matches.Any())
            {
                var solutionPath = Path.GetDirectoryName(solutionFile);
                var candidates = matches.Select(x => x.Groups[2].Value).ToList();
                var assetFiles = new List<string>(candidates.Count);
                for (var i = 0; i < candidates.Count; ++i)
                {
                    candidates[i] = candidates[i].Replace('\\', Path.DirectorySeparatorChar);
                    if (!Path.IsPathRooted(candidates[i]))
                    {
                        candidates[i] = Path.Combine(solutionPath, candidates[i]);
                    }

                    candidates[i] = Path.GetFullPath(candidates[i]);

                    var assetFile = Directory.EnumerateFiles(Path.Combine(Path.GetDirectoryName(candidates[i]), _objFolderName), _assetsFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (assetFile != null)
                    {
                        assetFiles.Add(assetFile);
                    }
                }

                return assetFiles;
            }

            return Array.Empty<string>();
        }
    }
}
