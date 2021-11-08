using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.DevOps.Builds
{

    public class AzureDevOpsPipelineStatusCheck : AsyncStatusCheck<AzureDevOpsPipelineWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        private readonly IMapper _mapper;

        public AzureDevOpsPipelineStatusCheck(IMapper mapper) => _mapper = mapper;

        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AzureDevOpsPipelineWidget> request, CancellationToken cancellationToken)
        {
            var api = new AzureDevOpsApi(Endpoint);

            var builds = await api.GetBuildsAsync(request.Context.Account, request.Context.Project, request.Context.DefinitionId, top: 1).ConfigureAwait(false);

            var build = builds?.Value?.FirstOrDefault();

            if (build is null)
            {
                request.Context.Reset();
                request.Context.Status = Status.Unknown;
            }
            else
            {
                _mapper.Map(build, request.Context);
            }
        }
    }
}
