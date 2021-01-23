using MediatR;
using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class StartScheduler
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public Response(bool isStarted) => IsStarted = isStarted;

            public bool IsStarted { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IJobFactory _jobFactory;
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
            {
                _jobFactory = jobFactory;
                _schedulerFactory = schedulerFactory;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                scheduler.JobFactory = _jobFactory;

                if (!scheduler.IsStarted)
                {
                    await scheduler.Start(cancellationToken).ConfigureAwait(false);
                }

                return new Response(scheduler.IsStarted);
            }
        }
    }
}
