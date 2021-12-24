using AnyStatus.API.Widgets;
using System;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class Deployment
    {
        public string Description => Duration.ToString();

        public string DeploymentStatus { get; set; }

        public double Percentage { get; set; } = 100;

        public DateTime StartedOn { get; set; }

        public DateTime CompletedOn { get; set; }

        public TimeSpan Duration => CompletedOn - StartedOn;

        public Environment ReleaseEnvironment { get; set; }

        public string State
        {
            get => DeploymentStatus switch
            {
                "all" => Status.OK,
                "failed" => Status.Failed,
                "inProgress" => Status.Running,
                "notDeployed" => Status.None,
                "partiallySucceeded" => Status.PartiallySucceeded,
                "succeeded" => Status.OK,
                "undefined" => Status.Unknown,
                _ => Status.None,
            };
        }
    }
}
