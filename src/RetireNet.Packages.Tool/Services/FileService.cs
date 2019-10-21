using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;
using RetireNet.Packages.Tool.Extensions;

namespace RetireNet.Packages.Tool.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public LockFile ReadLockFile()
        {

            string assetsFile = null;
            try
            {
                assetsFile = Directory.EnumerateFiles(PathOfAssetsFile(), NameOfAssetsJsonFile(), SearchOption.TopDirectoryOnly).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Could not find project.assets.json file at '{PathOfAssetsFile()}'");
                _logger.LogDebug(e.ToString());
            }

            if (assetsFile != null)
            {
                _logger.LogDebug($"Found project.assets.json file at '{assetsFile}'".Green());
                return LockFileUtilities.GetLockFile(assetsFile, new NuGetLogger(_logger));
            }

            throw new NoAssetsFoundException();
        }


        public virtual string NameOfAssetsJsonFile()
        {
            return "project.assets.json";
        }

        public virtual string PathOfAssetsFile()
        {
            return Path.Combine(GetCurrentDirectory(), "obj");
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
