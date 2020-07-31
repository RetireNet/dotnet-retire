using System;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models.Reporting
{
    public class Project : IEquatable<Project>
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public IList<ProjectIssue> Issues { get; } = new List<ProjectIssue>();

        public new string ToString()
        {
            return $"{Name} ({Path})";
        }

        public bool Equals(Project other)
        {
            return ToString().Equals(other?.ToString());
        }

        public bool Equals(Project x, Project y)
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

            var p = (Project) obj;
            return Equals(this, p);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Path);
        }
    }
}
