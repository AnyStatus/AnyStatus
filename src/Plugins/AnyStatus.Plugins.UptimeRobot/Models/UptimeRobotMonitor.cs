namespace AnyStatus.Plugins.UptimeRobot.Models
{
    internal class UptimeRobotMonitor
    {
        public string Id { get; set; }
        public string FriendlyName { get; set; }
        public byte Status { get; set; }
        public string GetStatus() => Status switch
        {
            0 => AnyStatus.API.Widgets.Status.Paused,  //paused
            1 => AnyStatus.API.Widgets.Status.None,    //not checked
            2 => AnyStatus.API.Widgets.Status.OK,      //up
            8 => AnyStatus.API.Widgets.Status.Warning, //seems down
            9 => AnyStatus.API.Widgets.Status.Failed,  //down
            _ => AnyStatus.API.Widgets.Status.Unknown, //unknown
        };
    }
}
