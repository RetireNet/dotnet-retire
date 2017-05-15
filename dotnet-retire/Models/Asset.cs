using System.Collections.Generic;

namespace dotnet_retire
{
    public class Asset
    {
        public Asset()
        {
            Dependencies = new List<Asset>();
        }
        public string Id { get; set; }
        public string Version { get; set; }

        public List<Asset> Dependencies { get; set; }

        public override string ToString()
        {
            return $"{Id}/{Version}";
        }
    }
}