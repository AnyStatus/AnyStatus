using AnyStatus.API.Widgets;
using System;
using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class Build
    {
        public string Id { get; set; }

        public string BuildNumber { get; set; }

        public string Status { get; set; }

        public string Result { get; set; }

        public Status GetStatus => Status switch
        {
            "notStarted" => AnyStatus.API.Widgets.Status.Queued,
            "inProgress" => AnyStatus.API.Widgets.Status.Running,
            _ => Result switch
            {
                "notStarted" => AnyStatus.API.Widgets.Status.Running,
                "succeeded" => AnyStatus.API.Widgets.Status.OK,
                "failed" => AnyStatus.API.Widgets.Status.Failed,
                "canceled" => AnyStatus.API.Widgets.Status.Canceled,
                "partiallySucceeded" => AnyStatus.API.Widgets.Status.PartiallySucceeded,
                _ => AnyStatus.API.Widgets.Status.Unknown,
            },
        };

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public Dictionary<string, Dictionary<string, string>> Links { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (GetStatus == AnyStatus.API.Widgets.Status.Queued)
                {
                    return TimeSpan.MinValue;
                }

                if (GetStatus == AnyStatus.API.Widgets.Status.Running)
                {
                    return DateTime.Now - StartTime;
                }

                return FinishTime - StartTime;
            }
        }

        public double Percentage { get; set; }

        public IdentityRef RequestedBy { get; set; }

        public string Reason { get; set; }

        public string SourceBranch { get; set; }
    }
}
