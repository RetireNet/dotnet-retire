﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dotnet_retire
{
    public static class ConfigurationExtensions
    {
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            new Program().Execute(args);
        }

        public Program()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            var options = new RetireServiceOptions();
            Configuration.GetSection("RetireServiceOptions").Bind(options);
            
            Services = new ServiceCollection()
                .AddLogging()
                .AddOptions()
                .Configure<RetireServiceOptions>(o => o.RootUrl = options.RootUrl)
                .AddSingleton<RetireApiClient>()
                .AddSingleton<FileService>()
                .AddSingleton<NugetReferenceService>()
                .AddSingleton<UsagesFinder>()
                .AddSingleton<RetireLogger>()
                .BuildServiceProvider();


            Services
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

        }

        public IServiceProvider Services { get; set; }

        public IConfigurationRoot Configuration { get; set; }

        public void Execute(string[] args)
        {
            var retireLogger = Services.GetService<RetireLogger>();
            retireLogger.LogPackagesToRetire();
        }
    }
}
