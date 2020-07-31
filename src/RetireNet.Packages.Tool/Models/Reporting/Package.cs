using System;

namespace RetireNet.Packages.Tool.Models.Reporting
{
    public class Package : IEquatable<Package>, ICloneable
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public override string ToString()
        {
            return $"{Name} (v{Version})";
        }

        public object Clone()
        {
            return new Package
            {
                Name = Name,
                Version = Version,
            };
        }

        public bool Equals(Package other)
        {
            return ToString().Equals(other?.ToString());
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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var p = (Package) obj;
            return Equals(this, p);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Version);
        }
    }
}
