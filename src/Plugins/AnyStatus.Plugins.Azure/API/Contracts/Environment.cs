using AnyStatus.API.Widgets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class Environment
    {
        public int Id { get; set; }

        public int DefinitionEnvironmentId { get; set; }

        public int ReleaseId { get; set; }

        public string Name { get; set; }

        public IEnumerable<Approval> PreDeployApprovals { get; set; }

        public string Status { get; set; }

        public Status GetStatus() => Status switch
        {
            "notStarted" => AnyStatus.API.Widgets.Status.None,
            "inProgress" => PreDeployApprovals != null && PreDeployApprovals.Any(k => k.Status != "approved") ? AnyStatus.API.Widgets.Status.None : AnyStatus.API.Widgets.Status.Running,
            "succeeded" => AnyStatus.API.Widgets.Status.OK,
            "canceled" => AnyStatus.API.Widgets.Status.Canceled,
            "rejected" => AnyStatus.API.Widgets.Status.Failed,
            "queued" => AnyStatus.API.Widgets.Status.Queued,
            "scheduled" => AnyStatus.API.Widgets.Status.None,
            "partiallySucceeded" => AnyStatus.API.Widgets.Status.PartiallySucceeded,
            _ => AnyStatus.API.Widgets.Status.None,
        };
    }
}