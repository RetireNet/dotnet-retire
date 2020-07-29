using System;
using System.Collections;
using System.Collections.Generic;

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

        public new string ToString()
        {
            return $"{Name} (v{Version})";
        }

        public bool Equals(Package x, Package y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.ToString().Equals(y.ToString());
        }
    }
}
