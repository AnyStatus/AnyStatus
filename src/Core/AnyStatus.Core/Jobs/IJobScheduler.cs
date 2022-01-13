using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public interface IJobScheduler
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);

        Task TriggerJobAsync(string id, CancellationToken cancellation);

        Task ScheduleJobAsync(string id, object data, CancellationToken cancellationToken);
    }
}
