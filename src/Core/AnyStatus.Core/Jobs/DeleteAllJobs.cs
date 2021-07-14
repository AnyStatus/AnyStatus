using MediatR;
using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class DeleteAllJobs
    {
        public class Request : IRequest
        {
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                await scheduler.Clear(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
