using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API.Sources
{
    internal class AzureDevOpsAccountSource : IAsyncItemsSource
    {
        private readonly IEndpointProvider _endpointProvider;

        public AzureDevOpsAccountSource(IEndpointProvider endpointProvider) => _endpointProvider = endpointProvider;

        public Task<IEnumerable<NameValueItem>> GetItemsAsync(object src) => src is IAzureDevOpsWidget widget ? GetItemsAsync(widget) : null;

        public async Task<IEnumerable<NameValueItem>> GetItemsAsync(IAzureDevOpsWidget widget) =>
            _endpointProvider.GetEndpoint<IAzureDevOpsEndpoint>(widget.EndpointId) switch
            {
                AzureDevOpsEndpoint endpoint => await GetAccounts(endpoint),
                AzureDevOpsOAuthEndpoint endpoint => await GetAccounts(endpoint),
                AzureDevOpsServerEndpoint endpoint => await GetCollections(endpoint),
                _ => null,
            };

        private static async Task<List<NameValueItem>> GetCollections(IAzureDevOpsEndpoint endpoint)
        {
            var api = new AzureDevOpsApi(endpoint);

            var collections = await api.GetCollectionsAsync();

            return collections?.Value?.Select(c => new NameValueItem(c.Name)).ToList();
        }

        private static async Task<List<NameValueItem>> GetAccounts(IAzureDevOpsEndpoint endpoint)
        {
            var api = new AzureDevOpsApi(endpoint);

            var profile = await api.GetProfileAsync();

            if (profile is null)
            {
                throw new AzureDevOpsException("User profile could not be found.");
            }

            var accounts = await api.GetAccountsAsync(profile.Id);

            return accounts?.Value?.Select(a => new NameValueItem(a.AccountName)).ToList();
        }
    }
}