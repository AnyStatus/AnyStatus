using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.Docker.Images
{
    [Browsable(false)]
    public class ReadOnlyDockerImage : Widget, IRequireEndpoint<DockerEndpoint>, IRemovable
    {
        public string ImageId { get; set; }

        public string EndpointId { get; set; }
    }
}
