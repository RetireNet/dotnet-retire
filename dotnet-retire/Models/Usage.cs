using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet_retire
{
    public class Usage
    {
        private readonly List<NugetReference> _referenceDepth;
        public NugetReference NugetReference => _referenceDepth[0];
        public Package Package { get; set; }

        public Usage()
        {
            _referenceDepth = new List<NugetReference>();
        }

        public void Add(NugetReference asset)
        {
            _referenceDepth.Add(asset);
        }

        public string OuterMostId => _referenceDepth[_referenceDepth.Count - 1].Id;
        public bool IsDirect => _referenceDepth.Count == 1;

        public Usage Copy()
        {
            var copy = new Usage {Package = Package};
            copy._referenceDepth.AddRange(_referenceDepth);
            return copy;
        }

        public string ReadPath()
        {
            return string.Join(" > ", _referenceDepth.Skip(1).Select(r => $"{r.Id}/{r.Version}"));
        }
    }
}
