using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.AppVeyor.API;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.AppVeyor
{
    public class AppVeyorBuildStatusCheck : AsyncStatusCheck<AppVeyorBuildWidget>, IEndpointHandler<AppVeyorEndpoint>
    {
        public AppVeyorEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AppVeyorBuildWidget> request, CancellationToken cancellationToken)
        {
            var api = new AppVeyorAPI(Endpoint);

            var response = await api.GetLastBuildAsync(request.Context.ProjectSlug, request.Context.Branch).ConfigureAwait(false);

            request.Context.Status = response?.GetBuildStatus();

            request.Context.URL = $"{Endpoint.Address}/project/{Uri.EscapeDataString(Endpoint.AccountName)}/{request.Context.ProjectSlug}"; //todo: move to initializer or IOpenWebPage handler
        }
    }
}
