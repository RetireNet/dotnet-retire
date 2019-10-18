using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace RetireRuntimeMiddleware.Middlewares.Models
{
    internal class AppRunTimeDetails
    {
        private AppRunTimeDetails()
        {

        }

        public string OsPlatform { get; set; }
        public string TargetFramework { get; set; }
        public string RuntimeVersion { get; set; }

        public static AppRunTimeDetails New()
        {
            return new AppRunTimeDetails
            {
                OsPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                TargetFramework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName,
                RuntimeVersion = GetRuntimeVersion()
            };
        }

        public static string GetRuntimeVersion() {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }
    }
}
