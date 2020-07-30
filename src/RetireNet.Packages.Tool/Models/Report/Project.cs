using System;
using System.Collections;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models.Report
{
    public class Project : Package
    {
        public string Path { get; set; }

        public bool Equals(Project other)
        {
            return String.Compare(Name, other?.Name, StringComparison.Ordinal)
                   + String.Compare(Path, other?.Path, StringComparison.Ordinal)
                   == 0;
        }

        public new string ToString()
        {
            return $"{Name} ({Path})";
        }


        public IList<PackageIssue> Issues { get; } = new List<PackageIssue>();
    }
}
