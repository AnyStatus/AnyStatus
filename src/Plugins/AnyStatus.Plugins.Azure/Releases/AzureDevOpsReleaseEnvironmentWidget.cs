using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.Releases
{
    [Browsable(false)]
    public class AzureDevOpsReleaseEnvironmentWidget : StatusWidget, IRequireEndpoint<IAzureDevOpsEndpoint>
    {
        public int DefinitionEnvironmentId { get; set; }

        public int ReleaseId { get; set; }

        public int DeploymentId { get; set; }

        public string EndpointId { get; set; }
    }
}