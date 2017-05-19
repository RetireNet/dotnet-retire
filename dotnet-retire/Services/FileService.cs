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
            var objDirectory = Path.Combine(currentDirectory, "obj");
            string assetsFile = null;
            try
            {
                
                assetsFile = Directory.EnumerateFiles(objDirectory, "project.assets.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not find project.assets.json file at '{objDirectory}'. Looking for project.lock.json instead".Orange());
                Console.WriteLine(e);
            }

            if (assetsFile != null)
            {
                Console.WriteLine($"Found project.assets.json file at '{assetsFile}'".Green());
                var fileContents = File.ReadAllText(assetsFile);
                return JObject.Parse(fileContents);
            }

            try
            {
                assetsFile = Directory.EnumerateFiles(currentDirectory, "project.lock.json", SearchOption.TopDirectoryOnly).FirstOrDefault();

                if (assetsFile != null)
                {
                    Console.WriteLine($"Found project.lock.json file at '{assetsFile}'".Green());
                    var fileContents = File.ReadAllText(assetsFile);
                    return JObject.Parse(fileContents);
                }
                else
                {
                    Console.WriteLine($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not find project.lock.json file in  '{currentDirectory}'".Orange());
                Console.WriteLine(e);
            }

            throw new NoAssetsFoundException();
        }
    }
}