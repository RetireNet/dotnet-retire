using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RetireNet.Packages.Tool.Models.Reporting
{
    public class ProjectIssue : IEquatable<ProjectIssue>
    {
        public string Description { get; set; }
        public string IssueUrl { get; set; }
        public Package ProblemPackage { get; set; }

        public IList<PackageChain> PackageChains { get; set; } = new List<PackageChain>();

        public override string ToString()
        {
            return $"{Description} ({IssueUrl}) in {ProblemPackage}";
        }

        public bool Equals(ProjectIssue other)
        {
            return ToString().Equals(other?.ToString());
        }

        public bool Equals(ProjectIssue x, ProjectIssue y)
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

            var i = (ProjectIssue) obj;
            return Equals(this, i);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, IssueUrl, ProblemPackage?.Name, ProblemPackage?.Version);
        }
    }
}
