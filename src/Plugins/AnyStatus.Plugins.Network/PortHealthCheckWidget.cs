using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.HealthChecks
{
    public enum NetworkProtocol
    {
        TCP,
        UDP
    }

    [DisplayName("Port Check")]
    [Category("Network")]
    [Description("Check the availability of an TCP or UDP endpoint")]
    public class PortHealthCheckWidget : StatusWidget, IPollable, ICommonWidget
    {
        [Required]
        [Description("The host name or IP address.")]
        public string Host { get; set; }

        [Required]
        [Description("The network protocol.")]
        public NetworkProtocol Protocol { get; set; }

        [Required]
        [DisplayName("Port Number")]
        [Description("A port number between 0 and 65535.")]
        [Range(0, ushort.MaxValue, ErrorMessage = "Port number must be a number between 0 and 65535.")]
        public int PortNumber { get; set; } = 80;
    }
}
