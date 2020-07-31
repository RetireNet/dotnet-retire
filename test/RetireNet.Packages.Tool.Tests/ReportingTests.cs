using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Models.Reporting;
using RetireNet.Packages.Tool.Services;
using Xunit;

namespace DotNetRetire.Tests
{
    public class ReportingTests
    {
        [Fact]
        void UniquenessWorksProperlyForPackages()
        {
            var list = new List<Package>
            {
                new Package {Name = "Foo", Version = "Bar"},
                new Package {Name = "Foo", Version = "Bar"},
            };

            Assert.Single(list.Distinct());
        }
    }
}
