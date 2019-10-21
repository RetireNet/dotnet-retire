using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RetireNet.Runtimes.Core.Clients.Models
{
    public class AppRunTimeDetails
    {
        private AppRunTimeDetails()
        {

        }
        public static AppRunTimeDetails Build(string runtime = null)
        {
            return new AppRunTimeDetails
            {
                Os = GetOperatingSystem(),
                OsPlatform = RuntimeInformation.OSDescription,
                OsArchitecture = RuntimeInformation.OSArchitecture.ToString(),
                OsBits = Environment.Is64BitOperatingSystem ? "64" : "32",
                AppTargetFramework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName,
                AppRuntimeVersion = runtime ?? GetNetCoreAppRuntimeVersion(),
                AppBits = Environment.Is64BitProcess ? "64" : "32",
            };
        }
        public string Os { get; set; }

        public string OsPlatform { get; set; }

        public string OsArchitecture { get; set; }

        public string OsBits { get; set; }

        public string AppTargetFramework { get; set; }
        public string AppRuntimeVersion { get; set; }

        public string AppBits { get; set; }

        private static string GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows.ToString();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux.ToString();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX.ToString();
            return string.Empty;
        }

        public static string GetNetCoreAppRuntimeVersion()
        {
            return GetRuntimeVersion("Microsoft.NETCore.App");
        }

        private static string GetRuntimeVersion(string framework)
        {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, framework);
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                return assemblyPath[netCoreAppIndex + 1];
            return null;
        }
    }
}
