//using AnyStatus.API.Widgets;
//using MediatR;
//using MediatR.Pipeline;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AnyStatus.Core.Pipeline.HealthCheck
//{
//    /// <summary>
//    /// Publish internal notification when health-check status change as a result of (any) request handling.
//    /// </summary>
//    public class HealthCheckPostProcessor<TRequest, TResponse, TContext> : IRequestPostProcessor<TRequest, TResponse>
//        where TRequest : HealthCheckRequest<TContext>
//        where TContext : class, IHealthCheck
//    {
//        private readonly IMediator _mediator;

//        public HealthCheckPostProcessor(IMediator mediator)
//        {
//            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
//        }

//        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
//        {
//            //if (request.DataContext.PreviousStatus != request.DataContext.Status)
//            //{
//            //    await _mediator.Publish(new WidgetStatusNotification<TContext>(request.DataContext), cancellationToken);
//            //}
//        }
//    }
//}
