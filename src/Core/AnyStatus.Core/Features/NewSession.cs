using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Sessions;
using AnyStatus.Core.Widgets;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Features
{
    public class NewSession : IRequest<object>
    {
        public class Request : IRequest<bool> { }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IAppContext _context;
            private readonly IJobScheduler _jobScheduler;

            public Handler(IJobScheduler jobScheduler, IAppContext context)
            {
                _context = context;
                _jobScheduler = jobScheduler;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                await _jobScheduler.ClearAsync(cancellationToken);

                _context.Session = new Session
                {
                    IsDirty = true,
                    Widget = new Root()
                };

                return true;
            }
        }
    }
}