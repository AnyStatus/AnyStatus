using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.Workflows
{
    public class GitHubActionsWorkflowStatus : AsyncStatusCheck<GitHubActionsWorkflowWidget>, IEndpointHandler<GitHubEndpoint>
    {
        public GitHubEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<GitHubActionsWorkflowWidget> request, CancellationToken cancellationToken)
        {
            var response = await new GitHubAPI(Endpoint).GetWorkflowRunsAsync(request.Context.Repository, request.Context.Workflow, request.Context.Branch);

            request.Context.Status = response.WorkflowRuns?.FirstOrDefault()?.GetStatus() ?? Status.Unknown;
        }
    }
}
