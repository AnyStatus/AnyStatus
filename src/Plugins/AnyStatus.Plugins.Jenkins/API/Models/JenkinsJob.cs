using Newtonsoft.Json;

namespace AnyStatus.Plugins.Jenkins.API.Models
{
    public class JenkinsJob
    {
        public string Name { get; set; }

        public string URL { get; set; }

        public string Result { get; set; }

        public JenkinsJobProgress Executor { get; set; }

        [JsonProperty("building")] public bool IsRunning { get; set; }

        public string Status => IsRunning ? AnyStatus.API.Widgets.Status.Running : Result switch
        {
            "SUCCESS" => AnyStatus.API.Widgets.Status.OK,
            "ABORTED" => AnyStatus.API.Widgets.Status.Canceled,
            "FAILURE" => AnyStatus.API.Widgets.Status.Failed,
            "UNSTABLE" => AnyStatus.API.Widgets.Status.PartiallySucceeded,
            "QUEUED" => AnyStatus.API.Widgets.Status.Queued,
            _ => AnyStatus.API.Widgets.Status.Unknown
        };
    }
}