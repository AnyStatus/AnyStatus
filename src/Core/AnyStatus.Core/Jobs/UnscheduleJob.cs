using AnyStatus.API.Widgets;
using MediatR;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class UnscheduleJob
    {
        public class Request : IRequest
        {
            public Request(IWidget widget, bool includeChildren = false)
            {
                Widget = widget ?? throw new ArgumentNullException(nameof(widget));
            }

            public IWidget Widget { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                await Unschedule(request.Widget, scheduler, cancellationToken).ConfigureAwait(false);
            }

            private static async Task Unschedule(IWidget widget, IScheduler scheduler, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    _ = await scheduler.DeleteJob(new JobKey(widget.Id), cancellationToken).ConfigureAwait(false);
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Unschedule(child, scheduler, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
