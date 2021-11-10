using AnyStatus.Plugins.AppVeyor.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.AppVeyor.API
{
    internal class AppVeyorAPI
    {
        private readonly IRestClient _client;
        private readonly AppVeyorEndpoint _endpoint;

        public AppVeyorAPI(AppVeyorEndpoint endpoint)
        {
            _endpoint = endpoint;

            _client = new RestClient(_endpoint.Address)
            {
                Authenticator = endpoint.GetAuthenticator()
            };
        }

        private async Task<T> ExecuteAsync<T>(IRestRequest request)
        {
            var response = await _client.ExecuteAsync<T>(request).ConfigureAwait(false);

            return response.IsSuccessful && response.ErrorException is null ?
                response.Data :
                throw new Exception("An error response received from AppVeyor server. Response Status: " + response.StatusCode, response.ErrorException);
        }

        public Task<IEnumerable<AppVeyorProject>> GetProjectsAsync()
        {
            var request = new RestRequest($"/api/account/{Uri.EscapeDataString(_endpoint.AccountName)}/projects");

            return ExecuteAsync<IEnumerable<AppVeyorProject>>(request);
        }

        public Task<AppVeyorBuildResponse> GetLastBuildAsync(string projectSlug, string branch)
        {
            var resource = string.IsNullOrEmpty(branch) ?
                $"/api/projects/{Uri.EscapeDataString(_endpoint.AccountName)}/{projectSlug}" :
                $"/api/projects/{Uri.EscapeDataString(_endpoint.AccountName)}/{projectSlug}/branch/{branch}";

            var request = new RestRequest(resource);

            return ExecuteAsync<AppVeyorBuildResponse>(request);
        }
    }
}
