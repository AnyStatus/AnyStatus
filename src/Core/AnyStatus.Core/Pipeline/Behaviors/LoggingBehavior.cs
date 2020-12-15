using MediatR;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Pipeline.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                return await next().ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogTrace("{request} completed in {elapsed}.", request.GetType().ToFriendlyName(), stopwatch.Elapsed);
            }
        }
    }
}
