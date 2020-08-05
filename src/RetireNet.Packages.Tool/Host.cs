using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RetireNet.Packages.Tool.Services;
using RetireNet.Packages.Tool.Services.DotNet;
using RetireNet.Packages.Tool.Services.Reporting;

namespace RetireNet.Packages.Tool
{
    public class Host : IDisposable
    {
        private ServiceProvider _services;

        public Host Build(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddCommandLine(args, new Dictionary<string, string>
            {
                { "-p", "path" },
                { "--path", "path" },
                { "--ignore-failures", "ignore-failures" },
                { "--report-path", "report-path" },
                { "--report-format", "report-format" },
                { "--no-restore", "no-restore" },
            });
            var config = builder.Build();

            var alwaysExitWithZero = config.GetValue<bool?>("ignore-failures") ?? args.Any(x => x.Equals("--ignore-failures", StringComparison.OrdinalIgnoreCase));
            var path = config.GetValue<string>("path")?.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar) ?? Directory.GetCurrentDirectory();
            var rootUrlFromConfig = config.GetValue<Uri>("RootUrl");
            var logLevel = config.GetValue<LogLevel>("LogLevel");
            var reportPath = config.GetValue<string>("report-path")?.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            var reportFormat = config.GetValue<string>("report-format") ?? "JSON";

            var collection = new ServiceCollection()
                    .AddLogging(c => c.AddConsole().AddDebug().SetMinimumLevel(logLevel))
                    .AddOptions()
                    .Configure<RetireServiceOptions>(o =>
                    {
                        o.RootUrl = rootUrlFromConfig;
                        o.Path = path;
                        o.AlwaysExitWithZero = alwaysExitWithZero;
                        o.ReportPath = reportPath;
                        o.ReportFormat = reportFormat;
                    })
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

        public Void Run()
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
