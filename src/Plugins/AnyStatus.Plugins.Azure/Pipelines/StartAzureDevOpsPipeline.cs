using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AnyStatus.Plugins.Azure.DevOps.Builds;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.Pipelines
{
    public class StartAzureDevOpsPipeline : AsyncStartRequestHandler<AzureDevOpsPipelineWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override Task Handle(StartRequest<AzureDevOpsPipelineWidget> request, CancellationToken cancellationToken)
            => new AzureDevOpsApi(Endpoint).QueueBuildAsync(request.Context.Account, request.Context.Project, request.Context.DefinitionId, cancellationToken);
    }
}
