using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.Docker.Images
{
    [Category("Docker")]
    [DisplayName("Docker Images")]
    [Description("View a list of docker containers")]
    [Redirect("AnyStatus.Plugins.Docker.Images.DockerImages")]
    public class DockerImagesWidget : StatusWidget, IStandardWidget, IPollable, IRequireEndpoint<DockerEndpoint>, IInitializable
    {
        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
