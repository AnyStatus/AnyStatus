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

        public async Task TriggerJobAsync(string id, CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            var jobKey = new JobKey(id);

            if (await scheduler.CheckExists(jobKey, cancellationToken))
            {
                await scheduler.TriggerJob(jobKey, cancellationToken);
            }
        }

        public async Task ScheduleJobAsync(string id, object data, CancellationToken cancellationToken)
        {
            var job = JobBuilder.Create<Job>().WithIdentity(id).Build();

            job.JobDataMap.Put("data", data);

            var trigger = TriggerBuilder.Create()
                .WithIdentity(id)
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        public async Task DeleteJobAsync(string id, CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            await scheduler.DeleteJob(new JobKey(id), cancellationToken);
        }

        public async Task ClearAsync(CancellationToken cancellationToken)
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            await scheduler.Clear(cancellationToken);
        }
    }
}
