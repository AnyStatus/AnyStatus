using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class TriggerJob
    {
        public class Request : IRequest
        {
            public Request(IWidget widget)
            {
                Widget = widget;
            }

            public IWidget Widget { get; }
        }


        class Validator : IValidator<Request>
        {
            public IEnumerable<ValidationResult> Validate(Request request)
            {
                if (request.Widget is null)
                {
                    yield return new ValidationResult("Widget is null.");
                }
            }
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

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Widget.IsEnabled)
                {
                    _logger.LogInformation("Refreshing '{widget}'...", request.Widget.Name);

                    await Trigger(request.Widget, cancellationToken).ConfigureAwait(false);
                }
            }

            private async Task Trigger(IWidget widget, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    var scheduler = await _schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);

                    var jobKey = new JobKey(widget.Id);

                    var jobExists = await scheduler.CheckExists(jobKey, cancellationToken).ConfigureAwait(false);

                    if (jobExists)
                    {
                        await scheduler.TriggerJob(jobKey, cancellationToken).ConfigureAwait(false);
                    }
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Trigger(child, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
