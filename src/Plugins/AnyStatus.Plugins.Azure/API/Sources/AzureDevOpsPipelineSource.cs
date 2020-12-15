using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API.Sources
{
    internal class AzureDevOpsPipelineSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public AzureDevOpsPipelineSource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(object source)
        {
            if (source is IAzureDevOpsWidget widget && !string.IsNullOrEmpty(widget.EndpointId) && !string.IsNullOrEmpty(widget.Account) && !string.IsNullOrEmpty(widget.Project))
            {
                var endpoint = _endpointProvider.GetEndpoint<IAzureDevOpsEndpoint>(widget.EndpointId);

                if (endpoint != null)
                {
                    var api = new AzureDevOpsApi(endpoint);

                    var pipelines = await api.GetBuildDefinitionsAsync(widget.Account, widget.Project).ConfigureAwait(false);

                    return pipelines?.Value?.Select(p => new NameValueItem(p.Name, p.Id)).ToList();
                }
            }

            return null;
        }
    }
}