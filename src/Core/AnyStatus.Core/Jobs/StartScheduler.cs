using MediatR;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class StartScheduler
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public bool IsStarted { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IJobFactory _jobFactory;
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
            {
                _jobFactory = jobFactory ?? throw new ArgumentNullException(nameof(jobFactory));
                _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
                
                scheduler.JobFactory = _jobFactory;

                if (!scheduler.IsStarted)
                {
                    await scheduler.Start(cancellationToken).ConfigureAwait(false);
                }

                return new Response
                {
                    IsStarted = scheduler.IsStarted
                };
            }
        }
    }
}
