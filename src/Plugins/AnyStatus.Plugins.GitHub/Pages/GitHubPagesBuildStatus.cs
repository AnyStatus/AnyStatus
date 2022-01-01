using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.Pages
{
    public class GitHubPagesBuildStatus : AsyncStatusCheck<GitHubPagesBuildStatusWidget>, IEndpointHandler<GitHubEndpoint>
    {
        public GitHubEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<GitHubPagesBuildStatusWidget> request, CancellationToken cancellationToken)
        {
            var response = await new GitHubAPI(Endpoint).GetPagesAsync(request.Context.Repository);

            request.Context.Status = response.Status switch
            {
                "queued" => Status.Queued,
                "building" => Status.Running,
                "built" => Status.OK,
                "errored" => Status.Failed,
                _ => null
            };

            request.Context.URL = response.HtmlUrl;
        }
    }
}
