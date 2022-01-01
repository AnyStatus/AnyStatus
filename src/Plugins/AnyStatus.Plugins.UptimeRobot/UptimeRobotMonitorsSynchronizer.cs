using AnyStatus.API.Common;
using AnyStatus.Plugins.UptimeRobot.Models;

namespace AnyStatus.Plugins.UptimeRobot
{
    internal class UptimeRobotMonitorsSynchronizer : CollectionSynchronizer<UptimeRobotMonitor, UptimeRobotReadOnlyMonitorWidget>
    {
        public UptimeRobotMonitorsSynchronizer(UptimeRobotMonitorsWidget parent)
        {
            Compare = (monitor, widget) => monitor.Id.Equals(widget.MonitorId);

            Remove = widget => parent.Remove(widget);

            Update = (monitor, widget) =>
            {
                widget.Name = monitor.FriendlyName;
                widget.Status = monitor.GetStatus();
            };

            Add = monitor => parent.Add(
                new UptimeRobotReadOnlyMonitorWidget
                {
                    MonitorId = monitor.Id,
                    Name = monitor.FriendlyName,
                    Status = monitor.GetStatus()
                });
        }
    }
}
