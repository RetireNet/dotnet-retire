using System;
using RetireNet.Packages.Tool.Models.Report;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireServiceOptions
    {
        public Uri RootUrl { get; set; }

        public string Path { get; set; }

        public bool AlwaysExitWithZero { get; set; }
        public string ReportPath { get; set; }
        public String ReportFormat { get; set; }
        public bool NoRestore { get; set; }
        public bool NoColor { get; set; }
    }
}
