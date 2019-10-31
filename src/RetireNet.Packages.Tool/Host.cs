using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RetireNet.Packages.Tool.Services;
using RetireNet.Packages.Tool.Services.DotNet;

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
            });
            var config = builder.Build();


            var path = config.GetValue<string>("path");
            var rootUrlFromConfig = config.GetValue<Uri>("RootUrl");
            var logLevel = config.GetValue<LogLevel>("LogLevel");

            _services = new ServiceCollection()
                    .AddLogging(c => c.AddConsole().AddDebug().SetMinimumLevel(logLevel))
                    .AddOptions()
                    .Configure<RetireServiceOptions>(o =>
                    {
                        o.RootUrl = rootUrlFromConfig;
                        o.Path = path;
                    })
                    .AddTransient<RetireApiClient>()
                    .AddTransient<IFileService, FileService>()
                    .AddTransient<DotNetExeWrapper>()
                    .AddTransient<DotNetRunner>()
                    .AddTransient<DotNetRestorer>()
                    .AddTransient<IAssetsFileParser, NugetProjectModelAssetsFileParser>()
                    .AddTransient<UsagesFinder>()
                    .AddTransient<RetireLogger>()
                    .BuildServiceProvider();

            return this;
        }

        public void Run()
        {
            var retireLogger = _services.GetService<RetireLogger>();
            retireLogger.LogPackagesToRetire();
        }

        public void Dispose()
        {
            _services.Dispose();
        }
    }
}
