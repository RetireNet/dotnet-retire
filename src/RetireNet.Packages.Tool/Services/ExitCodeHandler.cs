using Microsoft.Extensions.Options;
using System;

namespace RetireNet.Packages.Tool.Services
{
    public class ExitCodeHandler : IExitCodeHandler
    {
        private readonly IOptions<RetireServiceOptions> _options;

        public ExitCodeHandler(IOptions<RetireServiceOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void HandleExitCode(int exitCode, bool exitImmediately = false)
        {
            if (!_options.Value.AlwaysExitWithZero)
            {
                Environment.ExitCode = exitCode;
            }

            if (exitImmediately)
            {
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}
