using System.Collections.Generic;
using System.Linq;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class PackageChain : List<Package>
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
    }
}
