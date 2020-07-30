using System.Collections;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class PackageIssue : Package
    {
        public string Description { get; set; }
        public string IssueUrl { get; set; }
        public Package ProblemPackage { get; set; }
        public IList<Project> Projects { get; set; } = new List<Project>();
        public IList<PackageChain> PackageChains { get; set; } = new List<PackageChain>();

        public new virtual string ToString()
        {
            return $"{Description} ({IssueUrl})";
        }
    }
}
