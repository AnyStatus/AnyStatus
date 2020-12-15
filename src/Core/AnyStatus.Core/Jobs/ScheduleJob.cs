using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
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
                Widget = widget ?? throw new ArgumentNullException(nameof(widget));
                IncludeChildren = includeChildren;
            }

            public IWidget Widget { get; }

            public bool IncludeChildren { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;
            private readonly ISchedulerFactory _schedulerFactory;

            public Handler(ILogger logger, ISchedulerFactory schedulerFactory)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                await Schedule(request.Widget, scheduler, request.IncludeChildren, cancellationToken).ConfigureAwait(false);
            }

            private async Task Schedule(IWidget widget, IScheduler scheduler, bool includeChildren, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    await Schedule(widget, scheduler, cancellationToken).ConfigureAwait(false);
                }

                if (includeChildren && widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Schedule(child, scheduler, true, cancellationToken).ConfigureAwait(false);
                    }
                }
            }

            private async Task Schedule(IWidget widget, IScheduler scheduler, CancellationToken cancellationToken)
            {
                var job = JobBuilder.Create<Job>().WithIdentity(widget.Id).Build();

                job.JobDataMap.Put(nameof(IWidget), widget);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(widget.Id)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                var test = await scheduler.GetJobDetail(new JobKey(widget.Id));

                await scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(false);

                _logger.LogDebug("Widget '{widget}' is running.", widget.Name);
            }
        }
    }
}
