using Microsoft.Extensions.Options;
using System;

namespace RetireNet.Packages.Tool.Services
{
    public class ExitCodeHandler
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
                if (exitImmediately)
                {
                    Environment.Exit(exitCode);
                }

                Environment.ExitCode = exitCode;
            }
        }
    }
}
