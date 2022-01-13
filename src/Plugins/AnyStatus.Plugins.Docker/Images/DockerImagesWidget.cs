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
    public class DockerImagesWidget : StatusWidget, ICommonWidget, IPollable, IRequireEndpoint<DockerEndpoint>, IInitializable
    {
        public DockerImagesWidget()
        {
            IsAggregate = true;
        }

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
