using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Models.Report;

namespace RetireNet.Packages.Tool.Services.Report
{
    public class MarkdownReportGenerator : IReportGenerator
    {
        public string Header { get; set; } = "# Retire Vulnerability Report";
        public string ProjectHeaderTemplate { get; set; } = "## {0} ({1})";
        public string IssueHeaderTemplate { get; set; } = "### {0} (v{1})";
        public string IssueDescriptionTemplate { get; set; } = " * [{0}]({1})";
        public string PackageTemplate { get; set; } = "* {0}";
        public string TargetPackageTemplate { get; set; } = "* **{0}**\n";
        public string PackageWithPaddingTemplate { get; set; } = "{1}{0}";
        public string Format => "Markdown";

        public string GenerateReport(Models.Report.Report report)
        {
            var markdown = new StringBuilder();
            markdown.AppendLine(Header);
            markdown.AppendLine();
            foreach (var project in report)
            {
                markdown.AppendLine(string.Format(ProjectHeaderTemplate, project.Name, project.Path));
                markdown.AppendLine();

                var groups =
                    project.Issues.GroupBy(i => string.Format(IssueHeaderTemplate, i.ProblemPackage.Name, i.ProblemPackage.Version));

                foreach (var packageGroup in groups)
                {
                    markdown.AppendLine(packageGroup.Key);
                    foreach (var issue in packageGroup)
                    {
                        markdown.AppendLine(string.Format(IssueDescriptionTemplate, issue.Description, issue.IssueUrl));
                        ProcessPackageChains(issue.PackageChains, issue.ProblemPackage, markdown);
                    }
                }
            }

            return markdown.ToString();
        }

        private void ProcessPackageChains(IList<IList<Package>> chains, Package targetPackage, StringBuilder markdown)
        {
            ProcessPackageChains(chains, targetPackage, markdown, 0);
        }

        private void ProcessPackageChains(IList<IList<Package>> chains, Package targetPackage, StringBuilder markdown, int layer)
        {
            // for some reason chain.GroupBy didn't do this... the result was groups of IList<Package>...
            var grouped = GroupByNextPackage(targetPackage, layer, chains);

            foreach (var group in grouped)
            {
                markdown.AppendLine(group.Key);
                ProcessPackageChains(group.Value, targetPackage, markdown, layer + 1);
            }
        }

        private Dictionary<string, IList<IList<Package>>> GroupByNextPackage(Package targetPackage, int layer, IList<IList<Package>> chains)
        {
            // for some reason chain.GroupBy didn't do this... the result was groups of IList<Package>...
            var grouped = new Dictionary<string, IList<IList<Package>>>();
            foreach (var chain in chains)
            {
                var target = chain.FirstOrDefault();
                if (target == null)
                {
                    continue;
                }

                var isTargetPackage = target.ToString() == targetPackage.ToString();
                var template = isTargetPackage ? TargetPackageTemplate : PackageTemplate;
                var targetKey = GetPackageLine(template, target, layer);

                if (grouped.ContainsKey(targetKey) == false)
                {
                    grouped[targetKey] = new List<IList<Package>>();
                }

                grouped[targetKey].Add(chain.Skip(1).ToList());
            }

            var sortedGroups = new Dictionary<string, IList<IList<Package>>>();

            foreach (var group in grouped)
            {
                sortedGroups[group.Key] = group.Value.OrderBy(chain => chain.Count()).ToList();
            }

            return sortedGroups;
        }

        private string GetPackageLine(String template, Package package, int layer)
        {
            var packageStr = string.Format(template, package.Name, package.Version);
            var withPadding = packageStr.PadLeft(packageStr.Length + (2 * layer) + 3, ' ');
            var withoutTemplate = withPadding.Replace(packageStr, "");
            return string.Format(PackageWithPaddingTemplate, packageStr, withoutTemplate);
        }
    }
}
