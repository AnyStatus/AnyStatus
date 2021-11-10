using AnyStatus.API.Widgets;

namespace AnyStatus.Plugins.AppVeyor.Models
{
    internal class AppVeyorBuildResponse
    {
        public AppVeyorBuild Build { get; set; }

        public Status GetBuildStatus() => Build?.Status switch
        {
            "success" => Status.OK,
            "failed" => Status.Failed,
            "failure" => Status.Failed,
            "cancelled" => Status.Canceled,
            "queued" => Status.Queued,
            "running" => Status.Running,
            _ => Status.Unknown,
        };
    }
}
