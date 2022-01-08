using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
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
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ILogger logger, ISchedulerFactory schedulerFactory)
            {
                _logger = logger;
                _schedulerFactory = schedulerFactory;
            }

            protected override Task Handle(Request request, CancellationToken cancellationToken) => TriggerJob(request.Widget, cancellationToken);

            private async Task TriggerJob(IWidget widget, CancellationToken cancellationToken)
            {
                if (widget is null)
                {
                    return;
                }

                _logger.LogInformation("Refreshing '{widget}'...", widget.Name);

                if (widget.IsEnabled && widget is IPollable)
                {
                    var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

                    var jobKey = new JobKey(widget.Id);

                    if (await scheduler.CheckExists(jobKey, cancellationToken))
                    {
                        await scheduler.TriggerJob(jobKey, cancellationToken);
                    }
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
