using System;
using System.Diagnostics;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    public class LogExceptionAction<TRequest> : RequestExceptionAction<TRequest>
    {
        private readonly ILogger _logger;

        public LogExceptionAction(ILogger logger) =>
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        protected override void Execute(TRequest request, Exception exception)
        {
#if DEBUG
            Debug.WriteLine(exception);
#endif
            _logger.LogError(exception, exception.Message);
        }
    }
}
