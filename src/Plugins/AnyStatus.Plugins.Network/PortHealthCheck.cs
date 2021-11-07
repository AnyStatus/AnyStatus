using AnyStatus.API.Widgets;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.HealthChecks
{
    public class PortHealthCheck : AsyncStatusCheck<PortHealthCheckWidget>
    {
        protected override Task Handle(StatusRequest<PortHealthCheckWidget> request, CancellationToken cancellationToken)
        {
            var protocol = request.Context.Protocol == NetworkProtocol.TCP ? ProtocolType.Tcp : ProtocolType.Udp;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol))
            {
                socket.Connect(request.Context.Host, request.Context.PortNumber);

                request.Context.Status = Status.OK;
            }

            return Task.CompletedTask;
        }
    }
}
