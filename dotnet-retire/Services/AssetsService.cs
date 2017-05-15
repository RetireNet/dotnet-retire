using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace dotnet_retire
{
    public static class AssetsService
    {
        public static Project GetAssets()
        {
            var projectAssetsJson = FileService.GetAssetsFile();
            var projectAssets = new Project();
            var jObject = JObject.Parse(projectAssetsJson);
            var targets = jObject.Property("targets").Value as JObject;

            foreach (var x in targets)
            {
                foreach (var child in x.Value.Children())
                {
                    var jprop = ((JProperty)child);
                    var asset = new Asset
                    {
                        Id = jprop.Name.Split('/')[0],
                        Version = jprop.Name.Split('/')[1],
                    };

                    var subDependencies = new List<Asset>();
                    foreach (var childOfAsset in child.Children())
                    {
                        var dependenciesNode = childOfAsset.Children().Where(c => ((JProperty)c).Name.Equals("dependencies"));
                        var subDeps = dependenciesNode.Children().Select(token =>
                        {
                            var assets = token.Select(d =>
                            {
                                var id = ((JProperty)d).Name.ToString();
                                var version = ((JProperty)d).Value.ToString();
                                return new Asset { Id = id, Version = version };
                            });
                            return assets;

                        });
                        subDependencies.AddRange(subDeps.SelectMany(c => c));

                    }
                    asset.Dependencies = subDependencies;
                    projectAssets.Assets.Add(asset);
                }
            }
            return projectAssets;
        }
    }
}