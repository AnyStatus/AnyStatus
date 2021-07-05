using Microsoft.Extensions.Logging;

namespace AnyStatus.Core.Logging
{
    public sealed class ActivityLoggerProvider : ILoggerProvider
    {
        private readonly ActivityLogger _logger;

        public ActivityLoggerProvider(ActivityLogger logger) => _logger = logger;

        public ILogger CreateLogger(string categoryName) => _logger;

        public void Dispose() { }
    }
}
