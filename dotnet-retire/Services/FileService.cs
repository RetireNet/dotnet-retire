using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace dotnet_retire
{
    public class FileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FileService>();
        }

        public JObject GetProjectAssetsJsonObject()
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
                _logger.LogWarning($"Could not find project.assets.json file at '{objDirectory}'. Looking for project.lock.json instead".Orange());
                _logger.LogDebug(e.ToString());
            }

            if (assetsFile != null)
            {
                _logger.LogInformation($"Found project.assets.json file at '{assetsFile}'".Green());
                var fileContents = File.ReadAllText(assetsFile);
                return JObject.Parse(fileContents);
            }

            try
            {
                assetsFile = Directory.EnumerateFiles(currentDirectory, "project.lock.json", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (assetsFile != null)
                {
                    _logger.LogInformation($"Found project.lock.json file at '{assetsFile}'".Green());
                    var fileContents = File.ReadAllText(assetsFile);
                    return JObject.Parse(fileContents);
                }
                else
                {
                    _logger.LogWarning($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                _logger.LogDebug(e.ToString());
            }

            throw new NoAssetsFoundException();
        }
    }
}