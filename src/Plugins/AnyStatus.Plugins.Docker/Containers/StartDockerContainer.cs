using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using Docker.DotNet.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Docker.Containers
{
    public class StartDockerContainer : AsyncStartRequestHandler<ReadOnlyDockerContainerWidget>, IEndpointHandler<DockerEndpoint>
    {
        public DockerEndpoint Endpoint { get; set; }

        protected override async Task Handle(StartRequest<ReadOnlyDockerContainerWidget> request, CancellationToken cancellationToken)
        {
            using var client = Endpoint.GetClient();

            await client.Containers.StartContainerAsync(request.Context.ContainerId, new ContainerStartParameters(), cancellationToken);
        }
    }
}
