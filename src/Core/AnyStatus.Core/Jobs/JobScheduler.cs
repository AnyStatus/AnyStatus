using Quartz;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class JobScheduler : IJobScheduler
    {
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobScheduler(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            scheduler.JobFactory = _jobFactory;

            if (scheduler.IsStarted)
            {
                return;
            }

            await scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            if (scheduler.IsShutdown)
            {
                return;
            }

            await scheduler.Shutdown(cancellationToken);
        }
    }
}
