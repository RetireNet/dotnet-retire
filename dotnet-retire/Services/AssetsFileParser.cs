using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace dotnet_retire
{
    public class AssetsFileParser
    {
        private readonly IFileService _fileService;

        public AssetsFileParser(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<NugetReference> GetNugetReferences()
        {
            var fileContents = _fileService.GetFileContents();
            var jObject = JObject.Parse(fileContents);
            var targets = jObject.Property("targets").Value as JObject;
            var assets = new List<NugetReference>();

            foreach (var x in targets)
            {
                foreach (var child in x.Value.Children())
                {
                    var jprop = (JProperty)child;
                    var nugetReference = new NugetReference
                    {
                        Id = jprop.Name.Split('/')[0],
                        Version = jprop.Name.Split('/')[1],
                    };

                    var dependencies = new List<NugetReference>();
                    foreach (var childOfAsset in child.Children())
                    {
                        var dependenciesNode = childOfAsset.Children().FirstOrDefault(c => ((JProperty)c).Name.Equals("dependencies"));

                        if (dependenciesNode != null)
                        {
                            var nugetReferences = GetDependencies(dependenciesNode);
                            dependencies.AddRange(nugetReferences);
                        }
                    }
                    nugetReference.Dependencies = dependencies;
                    assets.Add(nugetReference);
                }
            }
            return assets;
        }

        private static IEnumerable<NugetReference> GetDependencies(JToken dependenciesNode)
        {
            var subDeps = dependenciesNode.Children().Select(token =>
            {
                var dependency = token.Select(d =>
                {
                    var id = ((JProperty) d).Name.ToString();
                    var version = ((JProperty) d).Value.ToString();
                    return new NugetReference {Id = id, Version = version};
                });
                return dependency;
            });
            var allSubDeps = subDeps.SelectMany(c => c);
            return allSubDeps;
        }
    }
}