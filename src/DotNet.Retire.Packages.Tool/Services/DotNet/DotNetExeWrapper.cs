using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace DotNet.Retire.Packages.Tool.Services.DotNet
{
    /// <remarks>
    /// Based on https://github.com/natemcmaster/CommandLineUtils/blob/master/src/CommandLineUtils/Utilities/DotNetExe.cs
    /// </remarks>
    public class DotNetExeWrapper
    {
        private readonly ILogger<DotNetExeWrapper> _logger;
        private static string _fileName = "dotnet";

        public DotNetExeWrapper(ILogger<DotNetExeWrapper> logger)
        {
            _logger = logger;
        }

        public string DotNetExePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _fileName += ".exe";
            }

            var mainModule = Process.GetCurrentProcess().MainModule;
            if (!string.IsNullOrEmpty(mainModule?.FileName) && Path.GetFileName(mainModule.FileName).Equals(_fileName, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Module: " + mainModule.FileName);
                return mainModule.FileName;
            }

            var dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
            if (!string.IsNullOrEmpty(dotnetRoot))
            {
                _logger.LogDebug("Env variable: " + dotnetRoot);

                return Path.Combine(dotnetRoot, _fileName);
            }
            _logger.LogDebug("Not found!");

            return _fileName;
        }
    }
}
