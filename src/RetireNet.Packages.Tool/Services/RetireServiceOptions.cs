using System;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireServiceOptions
    {
        public Uri RootUrl { get; set; }

        public string Path { get; set; }

        public bool AlwaysExitWithZero { get; set; }
        public string ReportPath { get; set; }
        public string ReportFormat { get; set; }
    }
}
