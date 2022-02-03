using AnyStatus.API.Widgets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.HealthChecks
{
    public class PingHealthCheck : AsyncStatusCheck<PingHealthCheckWidget>
    {
        protected override async Task Handle(StatusRequest<PingHealthCheckWidget> request, CancellationToken cancellationToken)
        {
            using var ping = new Ping();

            var reply = await ping.SendPingAsync(request.Context.Host).ConfigureAwait(false);

            request.Context.Status = (reply.Status == IPStatus.Success) ? Status.OK : Status.Failed;
        }
    }
}
