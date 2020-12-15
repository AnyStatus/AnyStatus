using AnyStatus.API.Widgets;
using MediatR;
using System.Net.NetworkInformation;

namespace AnyStatus.Plugins.SystemInformation.Network
{
    public class ActiveTcpConnectionsQuery : RequestHandler<MetricRequest<ActiveTcpConnectionsWidget>>
    {
        protected override void Handle(MetricRequest<ActiveTcpConnectionsWidget> request)
        {
            request.Context.Value = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Length;

            request.Context.Status = Status.OK;
        }
    }
}
