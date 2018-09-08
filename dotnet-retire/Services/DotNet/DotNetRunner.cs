using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dotnet_retire
{
    /// <remarks>
    /// Copied shamelessly from https://github.com/jerriep/dotnet-outdated/tree/master/src/DotNetOutdated/Services
    /// Credit for the stuff happening in here goes to the https://github.com/jaredcnance/dotnet-status project
    /// </remarks>
    public class DotNetRunner
    {
        private readonly ILogger<DotNetRunner> _logger;
        private readonly string _dotnetPath;

        public DotNetRunner(DotNetExeWrapper wrapper,ILogger<DotNetRunner> logger)
        {
            _logger = logger;
            _dotnetPath = wrapper.DotNetExePath();
        }

        public RunStatus Run(string workingDirectory, string[] arguments)
        {
            _logger.LogDebug($"Path to `dotnet` : {_dotnetPath}");
            var psi = new ProcessStartInfo(_dotnetPath, string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var p = new Process();
            try
            {
                p.StartInfo = psi;
                p.Start();

                var output = new StringBuilder();
                var errors = new StringBuilder();
                var outputTask = ConsumeStreamReaderAsync(p.StandardOutput, output);
                var errorTask = ConsumeStreamReaderAsync(p.StandardError, errors);

                var processExited = p.WaitForExit(20000);

                if (processExited == false)
                {
                    p.Kill();

                    return new RunStatus(output.ToString(), errors.ToString(), exitCode: -1);
                }

                Task.WaitAll(outputTask, errorTask);

                return new RunStatus(output.ToString(), errors.ToString(), p.ExitCode);
            }
            finally
            {
                p.Dispose();
            }
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            await Task.Yield();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }
    }
}
