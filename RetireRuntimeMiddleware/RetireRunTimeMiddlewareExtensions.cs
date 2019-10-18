using Microsoft.AspNetCore.Builder;

namespace RetireRuntimeMiddleware
{
    public static class RetireRunTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseRuntimeVulnerabilityReport(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RetireRunTimeMiddleware>();
        }

    }
}
