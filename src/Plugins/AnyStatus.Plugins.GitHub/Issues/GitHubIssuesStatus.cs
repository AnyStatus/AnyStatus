using AnyStatus.API.Endpoints;
using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.Issues
{
    public class GitHubIssuesStatus : AsyncStatusCheck<GitHubIssuesWidget>, IEndpointHandler<GitHubEndpoint>
    {
        private readonly IMapper _mapper;
        private readonly IDispatcher _dispatcher;

        public GitHubIssuesStatus(IDispatcher dispatcher, IMapper mapper)
        {
            _mapper = mapper;
            _dispatcher = dispatcher;
        }

        public GitHubEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<GitHubIssuesWidget> request, CancellationToken cancellationToken)
        {
            var issues = await new GitHubAPI(Endpoint).GetIssuesAsync();

            if (issues is null || issues.Count == 0)
            {
                request.Context.Status = null;

                _dispatcher.InvokeAsync(request.Context.Clear);
            }
            else
            {
                request.Context.Text = issues.Count.ToString();

                _dispatcher.InvokeAsync(()
                    => new GitHubIssuesSynchronizer(_mapper, request.Context)
                            .Synchronize(issues, request.Context.OfType<GitHubIssueWidget>().ToList()));
            }
        }
    }
}
