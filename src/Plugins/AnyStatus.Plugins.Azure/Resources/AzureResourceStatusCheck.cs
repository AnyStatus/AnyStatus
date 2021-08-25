using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.Resources
{
    public class AzureResourceStatusCheck : AsyncStatusCheck<AzureResourceWidget>, IEndpointHandler<IAzureEndpoint>
    {
        private const string _availabilityStateKey = "availabilityState";

        public IAzureEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<AzureResourceWidget> request, CancellationToken cancellationToken)
        {
            if (request.Context.Location.Equals("global"))
            {
                return;
            }

            var api = new AzureApi(Endpoint);

            var resourceHealth = await api.GetResourceHealthAsync(request.Context.ResourceId);

            if (resourceHealth?.Properties != null && resourceHealth.Properties.TryGetValue(_availabilityStateKey, out var availabilityState))
            {
                request.Context.Status = availabilityState switch
                {
                    "Available" => Status.OK,
                    "Unknown" => Status.Unknown,
                    "Degraded" => Status.Failed,
                    "Unavailable" => Status.Stopped,
                    _ => Status.None
                };
            }
        }
    }
}
