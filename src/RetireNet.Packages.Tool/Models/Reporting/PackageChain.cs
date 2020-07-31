using System;
using System.Collections.Generic;
using System.Linq;

namespace RetireNet.Packages.Tool.Models.Reporting
{
    public class PackageChain : List<Package>, ICloneable
    {
        public PackageChain() : base()
        {
        }

        public PackageChain(IEnumerable<Package> packages) : base(packages)
        {
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(p => p.ToString()));
        }

        public object Clone()
        {
            return new PackageChain(this.Select(x => (Package) x.Clone()));
        }
    }
}
