using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RetireNet.Packages.Tool.Models.Report;

namespace RetireNet.Packages.Tool.Services.Report
{
    public class MarkdownInvertedReportGenerator : IReportGenerator
    {
        public string Header { get; set; } = "# Retire Vulnerability Report";
        public string PackageHeaderTemplate { get; set; } = "## {0} (v{1}) ({2} issue(s))";
        public string IssueDescriptionTemplate { get; set; } = " * [{0}]({1})";
        public string ProjectHeaderTemplate { get; set; } = "    #### {0} Affected Project(s)";
        public string ProjectTemplate { get; set; } = "     * {0} ({1})";
        public string ProjectDependenciesTemplate { get; set; } = "       From dependencies:";
        public string PackageTemplate { get; set; } = "* {0} (v{1})\n";
        public string TargetPackageTemplate { get; set; } = "* **{0} (v{1})**\n";
        public string PackageWithPaddingTemplate { get; set; } = "{1}{0}";
        public string Format => "Markdown-Inverted";

        public string GenerateReport(Models.Report.Report report)
        {
            var markdown = new StringBuilder();
            markdown.AppendLine(Header);
            markdown.AppendLine();

            var repackage = RepackageReport(report);

            foreach (var problemPackagePair in repackage)
            {
                var problemPackage = problemPackagePair.Key;

                markdown.AppendLine(string.Format(PackageHeaderTemplate, problemPackage.Name, problemPackage.Version, problemPackagePair.Value.Count()));
                markdown.AppendLine();

                foreach (var issue in problemPackagePair.Value)
                {
                    markdown.AppendLine(string.Format(IssueDescriptionTemplate, issue.Description, issue.IssueUrl));
                    markdown.AppendLine();
                    markdown.AppendLine(string.Format(ProjectHeaderTemplate, issue.Projects.Count()));
                    foreach (var project in issue.Projects)
                    {
                        markdown.AppendLine(string.Format(ProjectTemplate, project.Name, project.Path));
                        markdown.AppendLine(ProjectDependenciesTemplate);

                        var packageChains = issue.PackageChains.ToList<IList<Package>>();
                        var directIncludes = GroupByNextPackage(problemPackage, 2, packageChains);

                        foreach (var group in directIncludes)
                        {
                            markdown.AppendLine(group.Key);
                        }
                    }

                    markdown.AppendLine();
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

        private Dictionary<Package, List<PackageIssue>> RepackageReport(Models.Report.Report report)
        {
            var repackage = new Dictionary<string, Dictionary<string, PackageIssue>>();

            foreach (var project in report)
            {
                foreach (var issue in project.Issues)
                {
                    var packageKey = issue.ProblemPackage.ToString();
                    var issueKey = issue.IssueUrl;

                    if (repackage.ContainsKey(packageKey) == false)
                    {
                        repackage.Add(packageKey, new Dictionary<string, PackageIssue>());
                    }

                    if (repackage[packageKey].ContainsKey(issueKey) == false)
                    {
                        issue.Projects.Add(project);
                        repackage[packageKey][issueKey] = issue;
                        continue;
                    }

                    repackage[packageKey][issueKey].Projects.Add(project);

                    foreach (var chain in issue.PackageChains)
                    {
                        repackage[packageKey][issueKey].PackageChains.Add(chain);
                    }
                }
            }

            foreach (var packagePair in repackage)
            {
                foreach (var issuePair in packagePair.Value)
                {

                    issuePair.Value.PackageChains =
                        issuePair
                            .Value
                            .PackageChains
                            .GroupBy(pc => pc.ToString())
                            .Select(g => g.FirstOrDefault())
                            .ToList();

                    issuePair.Value.Projects =
                        issuePair
                            .Value
                            .Projects
                            .GroupBy(p => p.ToString())
                            .Select(g => g.FirstOrDefault())
                            .ToList();

                }
            }

            var ret = new Dictionary<Package, List<PackageIssue>>();

            foreach (var packagePair in repackage)
            {
                var list = new List<PackageIssue>();
                ret.Add(packagePair.Value.First().Value.ProblemPackage, list);

                foreach (var packageIssuePair in packagePair.Value)
                {
                    list.Add(packageIssuePair.Value);
                }
            }

            return ret;
        }
    }
}
