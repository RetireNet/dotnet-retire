using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RetireNet.Runtimes.Core;
using RetireNet.Runtimes.Core.Clients.Models;

namespace RetireNet.Runtimes.Middleware
{
    internal class RetireRunTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ReportGenerator _client;

        public RetireRunTimeMiddleware(RequestDelegate next)
        {
            _next = next;
            _client = new ReportGenerator();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var report = await _client.GetReport(AppRunTimeDetails.Build());

            var json = JsonSerializer.Serialize(report, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            });

            context.Response.OnStarting(state =>
            {
                context.Response.ContentType = "application/json";
                return Task.CompletedTask;
            }, null);

            await context.Response.WriteAsync(json);
        }
    }
}
