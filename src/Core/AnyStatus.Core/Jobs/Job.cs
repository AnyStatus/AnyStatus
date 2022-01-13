using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
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
            if (context.JobDetail.JobDataMap.ContainsKey("data") && context.JobDetail.JobDataMap["data"] is IWidget widget && widget.IsEnabled)
            {
                await TryExecuteAsync(widget);
            }
        }

        private async Task TryExecuteAsync(IWidget widget)
        {
            try
            {
                var request = widget switch
                {
                    IMetricWidget => MetricRequestFactory.Create((dynamic)widget),
                    IStatusWidget => StatusRequestFactory.Create((dynamic)widget),
                    _ => throw new NotSupportedException(widget.GetType().FullName + " is not supported by the job scheduler"),
                };

                await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing job for widget '{widget}'", widget.Name);
            }
        }
    }
}
