using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.UptimeRobot
{
    [Category("UptimeRobot")]
    [DisplayName("UptimeRobot Monitors")]
    [Description("View the status of monitors on UptimeRobot")]
    public class UptimeRobotMonitorsWidget : StatusWidget, IRequireEndpoint<UptimeRobotEndpoint>, IStandardWidget, IPollable
    {
        public UptimeRobotMonitorsWidget() => IsPersisted = false;

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
