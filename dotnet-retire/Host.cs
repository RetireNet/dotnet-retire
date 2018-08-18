using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
    public class Host
    {
        private IServiceProvider _services;

        public Host Build(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddCommandLine(args);
            var config = builder.Build();


            var rootUrlFromConfig = config.GetValue<Uri>("RootUrl");
            var logLevel = config.GetValue<LogLevel>("LogLevel");

            _services = new ServiceCollection()
                .AddLogging(c => c.AddConsole().AddDebug().SetMinimumLevel(logLevel))
                .AddOptions()
                .Configure<RetireServiceOptions>(o => o.RootUrl = rootUrlFromConfig)
                .AddTransient<RetireApiClient>()
                .AddTransient<IFileService,FileService>()
                .AddTransient<AssetsFileParser>()
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
    }
}
