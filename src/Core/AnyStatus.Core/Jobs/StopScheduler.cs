using MediatR;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class StopScheduler
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public bool IsShutdown { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ISchedulerFactory schedulerFactory)
            {
                _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                if (!scheduler.IsShutdown)
                {
                    await scheduler.Shutdown(cancellationToken).ConfigureAwait(false);
                }

                return new Response
                {
                    IsShutdown = scheduler.IsShutdown
                };
            }
        }
    }
}
