using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public interface IJobScheduler
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
