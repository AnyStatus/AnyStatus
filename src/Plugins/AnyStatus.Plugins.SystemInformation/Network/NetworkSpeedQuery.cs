using AnyStatus.API.Widgets;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.SystemInformation.Network
{
    public class NetworkSpeedQuery : AsyncMetricQuery<NetworkSpeedWidget>
    {
        protected override async Task Handle(MetricRequest<NetworkSpeedWidget> request, CancellationToken cancellationToken)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                throw new Exception("Network is not available.");
            }

            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(k => k.Id == request.Context.NetworkInterfaceId);

            if (networkInterface is null)
            {
                throw new Exception($"Network interface '{request.Context.NetworkInterfaceId}' could not be found.");
            }

            var startValue = networkInterface.GetIPv4Statistics();

            var starTime = DateTime.Now;

            await Task.Delay(1000).ConfigureAwait(false);

            var endValue = networkInterface.GetIPv4Statistics();

            var endTime = DateTime.Now;

            var totalBytes = request.Context.Direction == NetworkSpeedDirection.Upload ?
                endValue.BytesSent - startValue.BytesSent :
                endValue.BytesReceived - startValue.BytesReceived;

            request.Context.Value = totalBytes / (endTime - starTime).TotalSeconds;

            request.Context.Status = Status.OK;
        }
    }
}
