using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using RetireNet.Packages.Tool.Extensions;
using RetireNet.Packages.Tool.Models.Report;
using RetireNet.Packages.Tool.Services;
using RetireNet.Packages.Tool.Services.DotNet;
using RetireNet.Packages.Tool.Services.Report;

namespace RetireNet.Packages.Tool
{
    public class Host : IDisposable
    {
        private ServiceProvider _services;

        public Host Build(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddEnvironmentVariables("RETIRE_");
            builder.AddCommandLine(args, new Dictionary<string, string>
            {
                { "-p", "path" },
                { "--path", "path" },
                { "--ignore-failures", "ignore-failures" },
                { "--report-path", "report-path" },
                { "--report-format", "report-format" },
                { "--no-restore", "no-restore" },
                { "--no-color", "no-color" },
            });
            var config = builder.Build();

            var alwaysExitWithZero = config.GetValue<bool?>("ignore-failures") ?? args.Any(x => x.Equals("--ignore-failures", StringComparison.OrdinalIgnoreCase));
            var noRestore = config.GetValue<bool?>("no-restore") ?? args.Any(x => x.Equals("--no-restore", StringComparison.OrdinalIgnoreCase));
            var noColor = config.GetValue<bool?>("no-color") ?? args.Any(x => x.Equals("--no-color", StringComparison.OrdinalIgnoreCase));
            var path = config.GetValue<string>("path")?.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar) ?? Directory.GetCurrentDirectory();
            var rootUrlFromConfig = config.GetValue<Uri>("RootUrl");
            var logLevel = config.GetValue<LogLevel>("LogLevel");
            var reportPath = config.GetValue<string>("report-path")?.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            var reportFormat = config.GetValue<string>("report-format") ?? "JSON";

            StringExtensions.NoColor = noColor;

            var collection = new ServiceCollection()
                    .AddOptions()
                    .Configure<ConsoleLoggerOptions>(o => { o.DisableColors = noColor; })
                    .Configure<RetireServiceOptions>(o =>
                    {
                        o.RootUrl = rootUrlFromConfig;
                        o.Path = path;
                        o.AlwaysExitWithZero = alwaysExitWithZero;
                        o.ReportPath = reportPath;
                        o.ReportFormat = reportFormat;
                        o.NoRestore = noRestore;
                        o.NoColor = noColor;
                    })
                    .AddLogging(c => c.AddConsole().AddDebug().SetMinimumLevel(logLevel))
                    .AddTransient<RetireApiClient>()
                    .AddTransient<IFileService, FileService>()
                    .AddTransient<DotNetExeWrapper>()
                    .AddTransient<DotNetRunner>()
                    .AddTransient<DotNetRestorer>()
                    .AddTransient<IAssetsFileParser, NugetProjectModelAssetsFileParser>()
                    .AddTransient<UsagesFinder>()
                    .AddTransient<RetireLogger>()
                    .AddTransient<ReportWriter>()
                    .AddTransient<IExitCodeHandler, ExitCodeHandler>();

            collection.Scan(
                x =>
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly == null)
                    {
                        return;
                    }
                    var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                    var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                    x.FromAssemblies(assemblies)
                        .AddClasses(classes => classes.AssignableTo(typeof(IReportGenerator)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });

            _services = collection.BuildServiceProvider();

            return this;
        }

        public void Run()
        {
            var retireLogger = _services.GetService<RetireLogger>();
            var report = retireLogger.LogPackagesToRetire();

            if (report == null)
            {
                return;
            }

            var retireReporter = _services.GetService<ReportWriter>();
            retireReporter.WriteReport(report);
        }

        public void Dispose()
        {
            _services.Dispose();
        }
    }
}
