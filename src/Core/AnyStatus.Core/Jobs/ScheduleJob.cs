using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class ScheduleJob
    {
        public class Request : IRequest
        {
            public Request(IWidget widget, bool includeChildren = false)
            {
                Widget = widget;
                IncludeChildren = includeChildren;
            }

            public IWidget Widget { get; }

            public bool IncludeChildren { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;
            private readonly IJobScheduler _jobScheduler;

            public Handler(ILogger logger, IJobScheduler jobScheduler)
            {
                _logger = logger;
                _jobScheduler = jobScheduler;
            }

            protected override Task Handle(Request request, CancellationToken cancellationToken) => Schedule(request.Widget, request.IncludeChildren, cancellationToken);

            private async Task Schedule(IWidget widget, bool includeChildren, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    await _jobScheduler.ScheduleJobAsync(widget.Id, widget, cancellationToken);

                    _logger.LogDebug("Widget '{widget}' job is running...", widget.Name);
                }

                if (includeChildren && widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Schedule(child, true, cancellationToken);
                    }
                }
            }
        }
    }
}
