using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using Docker.DotNet.Models;

namespace AnyStatus.Plugins.Docker.Images
{
    internal class DockerImagesSynchronizer : CollectionSynchronizer<ImagesListResponse, ReadOnlyDockerImage>
    {
        public DockerImagesSynchronizer(DockerImagesWidget parent)
        {
            Compare = (container, widget) => container.ID.Equals(widget.ImageId);

            Remove = widget => parent.Remove(widget);

            Update = (response, widget) =>
            {
                widget.Status = Status.OK;
                widget.Name = response.GetName();

                if (widget.EndpointId != parent.EndpointId)
                {
                    widget.EndpointId = parent.EndpointId;
                }
            };

            Add = response => parent.Add(
                new ReadOnlyDockerImage
                {
                    ImageId = response.ID,
                    Name = response.GetName(),
                    Status = Status.OK,
                    EndpointId = parent.EndpointId
                });
        }
    }
}
