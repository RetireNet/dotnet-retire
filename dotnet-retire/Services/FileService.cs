using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public string GetFileContents()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var objDirectory = Path.Combine(currentDirectory, "obj");
            string assetsFile = null;
            try
            {

                assetsFile = Directory.EnumerateFiles(objDirectory, "project.assets.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Could not find project.assets.json file at '{objDirectory}'. Looking for project.lock.json instead".Orange());
                _logger.LogDebug(e.ToString());
            }

            if (assetsFile != null)
            {
                _logger.LogDebug($"Found project.assets.json file at '{assetsFile}'".Green());
                return File.ReadAllText(assetsFile);
            }

            try
            {
                assetsFile = Directory.EnumerateFiles(currentDirectory, "project.lock.json", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (assetsFile != null)
                {
                    _logger.LogDebug($"Found project.lock.json file at '{assetsFile}'".Green());
                    return File.ReadAllText(assetsFile);
                }
                else
                {
                    _logger.LogDebug($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                _logger.LogDebug(e.ToString());
            }

            throw new NoAssetsFoundException();
        }
    }
}
