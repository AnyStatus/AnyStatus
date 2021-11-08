using AnyStatus.API.Endpoints;
using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.Releases
{
    public class AzureDevOpsReleaseStatusCheck : AsyncStatusCheck<AzureDevOpsReleaseWidget>, IEndpointHandler<IAzureDevOpsEndpoint>
    {
        private readonly IDispatcher _dispatcher;

        public AzureDevOpsReleaseStatusCheck(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public IAzureDevOpsEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AzureDevOpsReleaseWidget> request, CancellationToken cancellationToken)
        {
            var api = new AzureDevOpsApi(Endpoint);

            var releases = await api.GetReleasesAsync(request.Context.Account, request.Context.Project, request.Context.DefinitionId, 1, cancellationToken);

            if (releases.Count > 0)
            {
                var release = releases.Value.First();

                request.Context.LastReleaseId = release.Id;

                _dispatcher.Invoke(() => UpdateEnvironments(request.Context, release));
            }
            else
            {
                request.Context.Status = Status.None;
            }
        }

        private static void UpdateEnvironments(AzureDevOpsReleaseWidget parent, Release release)
        {
            var removedEnvironments = parent.OfType<AzureDevOpsReleaseEnvironmentWidget>()
                .Where(w => release.Environments.All(e => e.DefinitionEnvironmentId != w.DefinitionEnvironmentId))
                .ToList();

            foreach (IWidget environment in removedEnvironments)
            {
                parent.Remove(environment);
            }

            foreach (var environment in release.Environments)
            {
                AddOrUpdateEnvironment(parent, environment);
            }
        }

        private static void AddOrUpdateEnvironment(AzureDevOpsReleaseWidget parent, Environment environment)
        {
            var widget = parent.OfType<AzureDevOpsReleaseEnvironmentWidget>()
                .FirstOrDefault(w => w.DefinitionEnvironmentId == environment.DefinitionEnvironmentId);

            if (widget is null)
            {
                parent.Add(new AzureDevOpsReleaseEnvironmentWidget
                {
                    Name = environment.Name,
                    Status = environment.GetStatus(),
                    DeploymentId = environment.Id,
                    ReleaseId = environment.ReleaseId,
                    DefinitionEnvironmentId = environment.DefinitionEnvironmentId,
                    EndpointId = parent.EndpointId
                });
            }
            else
            {
                widget.Status = environment.GetStatus();
                widget.DeploymentId = environment.Id;
                widget.EndpointId = parent.EndpointId;
            }
        }
    }
}
