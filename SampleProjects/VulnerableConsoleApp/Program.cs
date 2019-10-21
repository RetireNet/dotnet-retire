using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VulnerableConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging(c => c.AddDebug().AddConsole())
                .ConfigureServices(services => services.AddRetireRuntimeHostedService(c => c.CheckInterval = 5000))
                .Build();
            host.Run();
        }
    }
}
