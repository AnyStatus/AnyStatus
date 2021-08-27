using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.Resources
{
    public class AzureResourcesStatusCheck : AsyncStatusCheck<AzureResourcesWidget>, IEndpointHandler<AzureOAuthEndpoint>
    {
        private readonly IDispatcher _dispatcher;

        public AzureResourcesStatusCheck(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public AzureOAuthEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AzureResourcesWidget> request, CancellationToken cancellationToken)
        {
            var api = new AzureApi(Endpoint);

            var resources = await api.GetResourcesAsync(request.Context.SubscriptionId);

            if (resources is null)
            {
                request.Context.Status = Status.None;
            }
            else
            {
                _dispatcher.Invoke(() => new AzureResourcesSynchronizer(request.Context)
                    .Synchronize(resources.Value.ToList(), request.Context.OfType<AzureResourceWidget>().ToList()));
            }
        }
    }
}
