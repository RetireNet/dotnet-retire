using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace dotnet_retire
{
    public static class FileService
    {
        public static JObject GetProjectAssetsJsonObject()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string assetsFile = null;
            try
            {
                assetsFile = Directory.EnumerateFiles(currentDirectory, "\\obj\\project.assets.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
            }
            catch (Exception)
            {
                Console.WriteLine($"Could not find assets file. Looking for project.lock.json instead".Orange());
            }

            if (assetsFile != null)
            {
                Console.WriteLine($"Found assets file: {assetsFile}".Green());
                var fileContents = File.ReadAllText(assetsFile);
                return JObject.Parse(fileContents);
            }

            try
            {
                assetsFile = Directory.EnumerateFiles(currentDirectory, "project.lock.json", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (assetsFile != null)
                {
                    Console.WriteLine($"Found assets file: {assetsFile}".Green());
                    var fileContents = File.ReadAllText(assetsFile);
                    return JObject.Parse(fileContents);
                }
            }
            catch (Exception)
            {

            }

            throw new NoAssetsFoundException();
        }
    }
}