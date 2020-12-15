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

        public Status State
        {
            get
            {
                switch (DeploymentStatus)
                {
                    case "all":
                        return Status.OK;

                    case "failed":
                        return Status.Failed;

                    case "inProgress":
                        return Status.Running;

                    case "notDeployed":
                        return Status.None;

                    case "partiallySucceeded":
                        return Status.PartiallySucceeded;

                    case "succeeded":
                        return Status.OK;

                    case "undefined":
                        return Status.Unknown;

                    default:
                        return Status.None;
                }
            }
        }
    }
}
