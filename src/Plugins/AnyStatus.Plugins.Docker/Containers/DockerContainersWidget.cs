using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Docker.Containers
{
    [Category("Docker")]
    [DisplayName("Docker Containers")]
    [Description("View a list of docker containers")]
    public class DockerContainersWidget : StatusWidget, IStandardWidget, IPollable, IRequireEndpoint<DockerEndpoint>
    {
        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
