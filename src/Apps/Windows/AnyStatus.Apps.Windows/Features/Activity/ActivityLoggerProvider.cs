using Microsoft.Extensions.Logging;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public sealed class ActivityLoggerProvider : ILoggerProvider
    {
        private readonly ActivityLogger _logger;

        public ActivityLoggerProvider(ActivityLogger logger)
        {
            _logger = logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
        }
    }
}
