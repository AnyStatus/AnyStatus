using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.UptimeRobot
{
    [Category("UptimeRobot")]
    [DisplayName("UptimeRobot Monitors")]
    [Description("View the status of monitors on UptimeRobot")]
    public class UptimeRobotMonitorsWidget : StatusWidget, IRequireEndpoint<UptimeRobotEndpoint>, ICommonWidget, IPollable
    {
        public UptimeRobotMonitorsWidget()
        {
            IsAggregate = true;
        }

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
