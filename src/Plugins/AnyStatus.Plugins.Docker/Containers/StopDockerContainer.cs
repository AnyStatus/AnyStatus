using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using Docker.DotNet.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Docker.Containers
{
    public class StopDockerContainer : AsyncStopRequestHandler<ReadOnlyDockerContainerWidget>, IEndpointHandler<DockerEndpoint>
    {
        public DockerEndpoint Endpoint { get; set; }

        protected override async Task Handle(StopRequest<ReadOnlyDockerContainerWidget> request, CancellationToken cancellationToken)
        {
            using var client = Endpoint.GetClient();

            await client.Containers.StopContainerAsync(request.Context.ContainerId, new ContainerStopParameters(), cancellationToken);
        }
    }
}
