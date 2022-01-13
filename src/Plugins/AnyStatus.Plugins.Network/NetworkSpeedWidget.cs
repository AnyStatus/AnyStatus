using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.SystemInformation.Network
{
    [Category("Network")]
    [DisplayName("Network Speed")]
    [Description("View the network download or upload speed")]
    public class NetworkSpeedWidget : MetricWidget, IPollable, ICommonWidget
    {
        [Required]
        [DisplayName("Network Interface")]
        [ItemsSource(typeof(NetworkInterfacesSource), autoload: true)]
        public string NetworkInterfaceId { get; set; }

        public NetworkSpeedDirection Direction { get; set; }

        public override string ToString() => BytesFormatter.Format(Convert.ToInt64(Value));
    }
}
