using System;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class Package : IEquatable<Package>
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public bool Equals(Package other)
        {
            return String.Compare(Name, other?.Name, StringComparison.Ordinal)
                + String.Compare(Version, other?.Version, StringComparison.Ordinal)
                == 0;
        }
    }
}
