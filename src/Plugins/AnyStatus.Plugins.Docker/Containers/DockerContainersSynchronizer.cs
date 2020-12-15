using AnyStatus.API.Common;
using Docker.DotNet.Models;

namespace AnyStatus.Plugins.Docker.Containers
{
    internal class DockerContainersSynchronizer : CollectionSynchronizer<ContainerListResponse, ReadOnlyDockerContainerWidget>
    {
        public DockerContainersSynchronizer(DockerContainersWidget parent)
        {
            Compare = (container, widget) => container.ID.Equals(widget.ContainerId);

            Remove = widget => parent.Remove(widget);

            Update = (container, widget) =>
            {
                widget.Name = container.GetName();
                widget.Status = container.GetStatus();

                if (widget.EndpointId != parent.EndpointId)
                {
                    widget.EndpointId = parent.EndpointId;
                }
            };

            Add = container => parent.Add(
                new ReadOnlyDockerContainerWidget
                {
                    ContainerId = container.ID,
                    Name = container.GetName(),
                    Status = container.GetStatus(),
                    EndpointId = parent.EndpointId
                });
        }
    }
}
