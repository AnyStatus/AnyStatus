//using MediatR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//#warning run only when needed. Replace with post-request handler.
//namespace AnyStatus.Core.Pipeline.Behaviors
//{
//    [Obsolete]
//    public class TriggersBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//    {
//        private readonly ILogger _logger;

//        public TriggersBehavior(ILogger logger)
//        {
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//        {
//            return next();
//        }
//    }
//}
