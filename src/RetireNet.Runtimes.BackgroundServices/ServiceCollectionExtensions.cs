using System;
using RetireNet.Runtimes.BackgroundServices;
using RetireNet.Runtimes.Core;

// ReSharper disable once CheckNamespace
// On purpose to avoid cluttering hosts with new package namespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRetireRuntimeHostedService(this IServiceCollection services, Action<RetireRuntimeBackgroundServiceOptions> configurator = null)
        {
            if (configurator == null)
            {
                services.Configure<RetireRuntimeBackgroundServiceOptions>(c =>
                {
                    var oneHourInMillis = 60 * 60 * 1000;
                    c.CheckInterval = oneHourInMillis;
                });
            }
            else
            {
                services.Configure(configurator);
            }

            services.AddSingleton<ReportGenerator>();
            return services.AddHostedService<RetireRuntimeBackgroundService>();
        }
    }
}
