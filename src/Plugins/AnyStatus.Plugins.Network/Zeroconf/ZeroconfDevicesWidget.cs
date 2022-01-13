using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.SystemInformation.Network.Zeroconf
{
    [Category("Network")]
    [DisplayName("Network Devices")]
    [Description("Discover devices in the network using Zeroconf (Bonjour) protocol")]
    public class ZeroconfDevicesWidget : StatusWidget, ICommonWidget, IPollable
    {
    }
}
