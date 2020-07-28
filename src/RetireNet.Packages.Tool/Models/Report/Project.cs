using System.Collections;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public IList<PackageIssue> Issues { get; } = new List<PackageIssue>();
    }
}
