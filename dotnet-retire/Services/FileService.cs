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
            var currentDirectory = Directory.GetCurrentDirectory() + "\\obj\\";
            var assetsFile = Directory.EnumerateFiles(currentDirectory, "project.assets.json", SearchOption.TopDirectoryOnly).FirstOrDefault();


            if (assetsFile == null)
            {
                Console.WriteLine("No assets file. Run dotnet restore first. ".Red());
                Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine($"Found assets file: {assetsFile}".Green());
            }
            
            var fileContents = File.ReadAllText(assetsFile);
            return JObject.Parse(fileContents);
        }
    }
}