using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.DevOps.Builds;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.Pipelines
{
    public class StopAzureDevOpsPipeline : AsyncStopRequestHandler<AzureDevOpsPipelineWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override Task Handle(StopRequest<AzureDevOpsPipelineWidget> request, CancellationToken cancellationToken)
            => new AzureDevOpsApi(Endpoint).CancelBuildAsync(request.Context.Account, request.Context.Project, request.Context.BuildId, cancellationToken);
    }
}
