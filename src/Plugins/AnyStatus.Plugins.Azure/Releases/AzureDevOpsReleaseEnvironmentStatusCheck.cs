//using AnyStatus.API.Widgets;
//using AnyStatus.Plugins.Azure.API;
//using AnyStatus.Plugins.Azure.API.Endpoints;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AnyStatus.Plugins.Azure.Releases
//{
//    class AzureDevOpsReleaseEnvironmentStatusCheck :
//        AsyncStatusCheck<AzureDevOpsReleaseEnvironmentWidget>,
//        IEndpointHandler<IAzureDevOpsEndpoint>
//    {
//        public IAzureDevOpsEndpoint Endpoint { get; set; }

//        protected override async Task Handle(StatusRequest<AzureDevOpsReleaseEnvironmentWidget> request, CancellationToken cancellationToken)
//        {
//            var widget = request.Context;

//            if (widget.Parent is AzureDevOpsReleaseWidget parent && parent.DefinitionId != 0)
//            {
//                var api = new AzureDevOpsApi(Endpoint);

//                var response = await api.GetDeploymentsAsync(parent.Project, parent.DefinitionId, widget.DefinitionEnvironmentId, 10, cancellationToken).ConfigureAwait(false);

//                var deployments = response.Value.ToList();

//                if (deployments.Any())
//                {
//                    deployments.Reverse();

//                    var maxDuration = deployments.Max(deployment => deployment.Duration);

//                    deployments.ForEach(deployment => deployment.Percentage = (double)deployment.Duration.Ticks / maxDuration.Ticks);
//                }

//                //widget.JobHistory = deployments;
//            }
//        }
//    }
//}
