using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class Refresh
    {
        public class Request : IRequest
        {
            public Request(IWidget widget) => Widget = widget;

            public IWidget Widget { get; }
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

            protected override Task Handle(Request request, CancellationToken cancellationToken) => TriggerJob(request.Widget, cancellationToken);

            private async Task TriggerJob(IWidget widget, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Refreshing '{widget}'...", widget.Name);

                if (widget.IsEnabled && widget is IPollable)
                {
                    await _jobScheduler.TriggerJobAsync(widget.Id, cancellationToken);
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await TriggerJob(child, cancellationToken);
                    }
                }
            }
        }
    }
}
