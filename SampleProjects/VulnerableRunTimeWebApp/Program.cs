using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace VulnerableRunTimeWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices((app, services) =>
                {
                    services.AddRetireRuntimeHostedService();
                })
                .Configure(app => app.UseRuntimeVulnerabilityReport())
                .UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .WriteTo.Console(new CompactJsonFormatter()));

    }
}
