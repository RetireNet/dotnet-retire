using System.Collections;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class PackageIssue : Package
    {
        public string Description { get; set; }
        public string IssueUrl { get; set; }
        public Package ProblemPackage { get; set; }
        public IList<IList<Package>> PackageChains { get; set; } = new List<IList<Package>>();
    }
}
