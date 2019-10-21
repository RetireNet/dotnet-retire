using DotNet.Retire.Runtimes.Middleware;

// ReSharper disable once CheckNamespace
// On purpose to avoid cluttering hosts with new package namespace
namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseRuntimeVulnerabilityReport(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RetireRunTimeMiddleware>();
        }
    }
}
