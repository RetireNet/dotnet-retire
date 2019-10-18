using RetireRuntimeMiddleware.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class RetireRunTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseRuntimeVulnerabilityReport(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RetireRunTimeMiddleware>();
        }
    }
}
