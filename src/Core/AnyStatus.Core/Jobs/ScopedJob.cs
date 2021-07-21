using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class ScopedJob : Job
    {
        private readonly Container _container;

        public ScopedJob(ILogger logger, IMediator mediator, Container container) : base(logger, mediator) => _container = container;

        public override async Task Execute(IJobExecutionContext context)
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                await base.Execute(context).ConfigureAwait(false);
            }
        }
    }
}
