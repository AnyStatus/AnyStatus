using Microsoft.Extensions.Logging;

namespace AnyStatus.Core.Logging
{
    public sealed class LoggerProvider : ILoggerProvider
    {
        private readonly Logger _logger;

        public LoggerProvider(Logger logger) => _logger = logger;

        public ILogger CreateLogger(string categoryName) => _logger;

        public void Dispose()
        {
            _logger.Dispose();
        }
    }
}
