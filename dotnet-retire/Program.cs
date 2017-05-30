using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
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

            var rootUrl = Configuration.GetValue<string>("RetireServiceOptions:RootUrl");
            Services = new ServiceCollection()
                .AddLogging()
                .AddOptions()
                .Configure<RetireServiceOptions>(o => o.RootUrl = rootUrl)
                .AddSingleton<RetireApiClient>()
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
