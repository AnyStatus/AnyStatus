using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.Resources
{
    [Browsable(false)]
    public class AzureResourceWidget : StatusWidget, IPollable, IRequireEndpoint<IAzureEndpoint>
    {
        public string EndpointId { get; set; }

        public string ResourceId { get; set; }

        public string Kind { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }
    }
}
