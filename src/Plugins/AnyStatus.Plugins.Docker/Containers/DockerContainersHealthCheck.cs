using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using Docker.DotNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Docker.Containers
{
    public class DockerContainersHealthCheck : AsyncStatusCheck<DockerContainersWidget>, IEndpointHandler<DockerEndpoint>
    {
        private readonly IDispatcher _dispatcher;

        public DockerContainersHealthCheck(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public DockerEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<DockerContainersWidget> request, CancellationToken cancellationToken)
        {
            ICollection<ContainerListResponse> containers;

            using (var client = Endpoint.GetClient())
            {
                containers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true }).ConfigureAwait(false);
            }

            if (containers is null)
            {
                request.Context.Status = Status.Unknown;
            }
            else if (containers.Count > 0)
            {
                _dispatcher.Invoke(() => new DockerContainersSynchronizer(request.Context).Synchronize(containers, request.Context.OfType<ReadOnlyDockerContainerWidget>().ToList()));
            }
            else
            {
                request.Context.Status = Status.OK;
            }
        }
    }
}
