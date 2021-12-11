using AnyStatus.API.Endpoints;
using System.ComponentModel;

namespace AnyStatus.Plugins.UptimeRobot
{
    [DisplayName("UptimeRobot")]
    public class UptimeRobotEndpoint : Endpoint
    {
        public UptimeRobotEndpoint()
        {
            Name = "UptimeRobot";
            Address = "https://api.uptimerobot.com";
        }

        [DisplayName("API Key")]
        [Description("The API key can be found at UptimeRobot under \"My Settings\" page")]
        public string APIKey { get; set; }
    }
}
