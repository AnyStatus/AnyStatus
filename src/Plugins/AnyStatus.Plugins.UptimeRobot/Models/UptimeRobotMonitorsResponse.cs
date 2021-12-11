using System.Collections.Generic;

namespace AnyStatus.Plugins.UptimeRobot.Models
{
    internal class UptimeRobotMonitorsResponse
    {
        public IEnumerable<UptimeRobotMonitor> Monitors { get; set; }
    }
}
