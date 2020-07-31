using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RetireNet.Packages.Tool.Models.Reporting
{
    public class ReportIssue : ProjectIssue
    {
        public ReportIssue(ProjectIssue issue)
        {
            Description = issue.Description;
            IssueUrl = issue.IssueUrl;
            ProblemPackage = (Package) issue.ProblemPackage.Clone();
            PackageChains =
                new List<PackageChain>(issue.PackageChains.Select(x => (PackageChain) x.Clone()));
        }

        public IList<Project> Projects { get; set; } = new List<Project>();
    }
}
