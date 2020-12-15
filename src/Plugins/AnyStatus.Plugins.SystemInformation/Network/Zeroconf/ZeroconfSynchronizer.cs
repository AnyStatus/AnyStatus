using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using Zeroconf;

namespace AnyStatus.Plugins.SystemInformation.Network.Zeroconf
{
    internal class ZeroconfSynchronizer : CollectionSynchronizer<IZeroconfHost, ZeroconfDeviceWidget>
    {
        public ZeroconfSynchronizer(ZeroconfDevicesWidget parent)
        {
            Compare = (host, widget) => widget.Name == GetHostName(host);

            Remove = widget => parent.Remove(widget);

            Update = (host, widget) =>
            {
                widget.Status = Status.OK;
            };

            Add = host => parent.Add(
                new ZeroconfDeviceWidget
                {
                    Name = GetHostName(host),
                    Status = Status.OK,
                });
        }

        private static string GetHostName(IZeroconfHost host) => $"{host.DisplayName} ({host.IPAddress})";
    }
}
