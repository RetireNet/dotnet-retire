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


            _services = new ServiceCollection()
                .AddLogging()
                .AddOptions()
                .Configure<RetireServiceOptions>(o => o.RootUrl = rootUrlFromConfig)
                .AddSingleton<RetireApiClient>()
                .AddSingleton<IFileService,FileService>()
                .AddSingleton<AssetsFileParser>()
                .AddSingleton<UsagesFinder>()
                .AddSingleton<RetireLogger>()
                .BuildServiceProvider();


            _services
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
            return this;
        }




        public void Run()
        {
            var retireLogger = _services.GetService<RetireLogger>();
            retireLogger.LogPackagesToRetire();
        }
    }
}
