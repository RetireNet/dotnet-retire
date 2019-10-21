using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using ILogger = NuGet.Common.ILogger;
using LogLevel = NuGet.Common.LogLevel;

namespace RetireNet.Packages.Tool.Services
{
    public class NuGetLogger : ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public NuGetLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string data)
        {
            _logger.LogDebug(data);
        }

        public void LogVerbose(string data)
        {
            _logger.LogTrace(data);
        }

        public void LogInformation(string data)
        {
            _logger.LogInformation(data);
        }

        public void LogMinimal(string data)
        {
            _logger.LogWarning(data);
        }

        public void LogWarning(string data)
        {
            _logger.LogWarning(data);
        }

        public void LogError(string data)
        {
            _logger.LogError(data);
        }

        public void LogInformationSummary(string data)
        {
            _logger.LogInformation(data);
        }

        public void Log(LogLevel level, string data)
        {
            Microsoft.Extensions.Logging.LogLevel mslevel;
            switch (level)
            {
                case LogLevel.Debug:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Debug;
                    break;
                case LogLevel.Verbose:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Debug;
                    break;
                case LogLevel.Information:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Information;
                    break;
                case LogLevel.Minimal:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Warning;
                    break;
                case LogLevel.Warning:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Warning;
                    break;
                case LogLevel.Error:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Error;
                    break;
                default:
                    mslevel = Microsoft.Extensions.Logging.LogLevel.Debug;
                    break;
            }
            _logger.Log(mslevel, data);
        }

        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message)
        {
            _logger.LogInformation(message.Message);
        }

        public Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
