﻿using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.API.Endpoints;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API
{
    internal class AzureApi
    {
        private readonly IRestClient _client;

        internal AzureApi(IAzureEndpoint endpoint)
        {
            if (endpoint is null)
            {
                throw new EndpointNotFoundException();
            }

            _client = new RestClient(endpoint.Address)
            {
                Authenticator = endpoint.GetAuthenticator()
            };
        }

        private Task<T> ExecuteAsync<T>(string resource, CancellationToken cancellationToken) where T : new() =>
            ExecuteAsync<T>(new RestRequest(resource), cancellationToken);

        private async Task<T> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken) where T : new()
        {
            request.AddParameter("api-version", "2020-05-01");

            //request.AddHeader("X-TFS-FedAuthRedirect", "Suppress");

            var response = await _client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.UnprocessableEntity)
            {
                throw new Exception("An error occurred while requesting data from Azure DevOps. " + response.StatusDescription, response.ErrorException);
            }

            return default;
        }

        internal Task<AzureCollectionResponse<Subscription>> GetSubscriptionsAsync(CancellationToken cancellationToken = default) =>
            ExecuteAsync<AzureCollectionResponse<Subscription>>("/subscriptions", cancellationToken);

        internal Task<AzureCollectionResponse<Resource>> GetResourcesAsync(string subscriptionId, CancellationToken cancellationToken = default) =>
            ExecuteAsync<AzureCollectionResponse<Resource>>($"/subscriptions/{subscriptionId}/resources", cancellationToken);

        internal Task<AvailabilityStatus> GetResourceHealthAsync(string resourceId, CancellationToken cancellationToken = default) =>
            ExecuteAsync<AvailabilityStatus>($"{resourceId}/providers/Microsoft.ResourceHealth/availabilityStatuses/current", cancellationToken);
    }
}
