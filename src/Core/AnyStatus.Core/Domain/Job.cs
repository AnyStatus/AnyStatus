using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AnyStatus.Core.Domain
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public Job(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (context.JobDetail.JobDataMap.ContainsKey(nameof(IWidget)) &&
                    context.JobDetail.JobDataMap[nameof(IWidget)] is IWidget widget &&
                    widget.IsEnabled)
                {
                    var request = widget switch
                    {
                        IMetricWidget _ => MetricRequestFactory.Create((dynamic)widget),

                        IStatusWidget _ => StatusRequestFactory.Create((dynamic)widget),

                        _ => throw new NotSupportedException($"{widget?.GetType().FullName} is not supported by the job scheduler."),
                    };

                    await _mediator.Send(request).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogError("An error occurred while executing job. Widget not found in job context.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while executing job.");
            }
        }
    }
}
