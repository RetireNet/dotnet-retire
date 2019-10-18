using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using DotNetCore.ReleaseMetadata.HttpClient.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RetireRuntimeMiddleware
{
    public class RetireRunTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ReleaseMetadataHttpClient _httpClient;

        public RetireRunTimeMiddleware(RequestDelegate next)
        {
            _next = next;
            _httpClient = new ReleaseMetadataHttpClient();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var runtimes = await _httpClient.GetVulnerableRuntimes();
            var runtimeVersion = GetRuntimeVersion();
            var report = new Report
            {
                AppRuntimeDetails = new AppRunTimeDetails
                {
                    OsPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                    TargetFramework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName,
                    RuntimeVersion = runtimeVersion

                },
                VulnerableRuntimes = runtimes.Vulnerables.FirstOrDefault(r => r.Runtime?.Version == runtimeVersion)
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(report));
        }

        public static string GetRuntimeVersion() {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }

        class Report
        {
            public AppRunTimeDetails AppRuntimeDetails { get; set; }
            public Release VulnerableRuntimes { get; set; }
        }
    }

    public class AppRunTimeDetails
    {
        public string OsPlatform { get; set; }
        public string TargetFramework { get; set; }

        public string FrameworkDescription { get; set; }
        public string RuntimeVersion { get; set; }
    }
}
