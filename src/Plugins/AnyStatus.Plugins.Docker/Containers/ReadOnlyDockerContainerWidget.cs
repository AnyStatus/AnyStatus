using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.Docker.Containers
{
    [Browsable(false)]
    public class ReadOnlyDockerContainerWidget : Widget, IRequireEndpoint<DockerEndpoint>, IStartable, IStoppable, IRestartable, IRemovable
    {
        public string ContainerId { get; set; }

        public string EndpointId { get; set; }

        public bool CanStart => Status != Status.OK;

        public bool CanStop => Status == Status.OK;
    }
}
