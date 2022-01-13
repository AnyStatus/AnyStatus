using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class DeleteAllJobs
    {
        public class Request : IRequest { }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IJobScheduler _jobScheduler;

            public Handler(IJobScheduler jobScheduler) => _jobScheduler = jobScheduler;

            protected override Task Handle(Request request, CancellationToken cancellationToken) => _jobScheduler.ClearAsync(cancellationToken);
        }
    }
}
