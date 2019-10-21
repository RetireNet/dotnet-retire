using System;
using System.Collections.Generic;

namespace RetireNet.Packages.Tool.Models
{
    public class Usage
    {
        private readonly List<NugetReference> _referenceDepth;
        public NugetReference NugetReference => _referenceDepth[0];
        public Package Package { get; set; }
        public Uri IssueUrl { get; set; }
        public string Description { get; set; }

        public Usage()
        {
            _referenceDepth = new List<NugetReference>();
        }

        public void Add(NugetReference asset)
        {
            _referenceDepth.Add(asset);
        }

        public NugetReference OuterMostPackage => _referenceDepth[_referenceDepth.Count - 1];

        public bool IsDirect => _referenceDepth.Count == 1;

        public Usage Copy()
        {
            var copy = new Usage
            {
                Package = Package,
                IssueUrl = IssueUrl,
                Description = Description
            };
            copy._referenceDepth.AddRange(_referenceDepth);
            return copy;
        }

        public string ReadPath()
        {
            var str = "";
            for (var i = _referenceDepth.Count-1; i >= 0; i--)
            {
                var whitespace = new string(' ', _referenceDepth.Count-i);
                var prefix = i == _referenceDepth.Count - 1 ? "" : "╚";
                str += $"\n {whitespace} {prefix} {_referenceDepth[i]}";
            }

            return str;
        }
    }
}
