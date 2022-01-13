using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Docker.Containers
{
    [Category("Docker")]
    [DisplayName("Docker Containers")]
    [Description("View a list of docker containers")]
    public class DockerContainersWidget : StatusWidget, ICommonWidget, IPollable, IRequireEndpoint<DockerEndpoint>
    {
        public DockerContainersWidget()
        {
            IsAggregate = true;
        }

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
