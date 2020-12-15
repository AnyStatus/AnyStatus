using AnyStatus.API.Widgets;
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

        public Status Status => IsRunning ? Status.Running : Result switch
        {
            "SUCCESS" => Status.OK,
            "ABORTED" => Status.Canceled,
            "FAILURE" => Status.Failed,
            "UNSTABLE" => Status.PartiallySucceeded,
            "QUEUED" => Status.Queued,
            _ => Status.Unknown
        };
    }
}