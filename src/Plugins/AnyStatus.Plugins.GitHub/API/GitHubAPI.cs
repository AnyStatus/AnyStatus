using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.GitHub.API.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.GitHub.API
{
    internal class GitHubAPI
    {
        private readonly IRestClient _client;

        public GitHubAPI(GitHubEndpoint endpoint)
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

        private async Task<T> ExecuteAsync<T>(IRestRequest request)
        {
            request.AddHeader("accept", "application/vnd.github.v3+json");

            var response = await _client.ExecuteAsync<T>(request).ConfigureAwait(false);

            return response.IsSuccessful && response.ErrorException is null ? response.Data : throw new Exception("An error response received from GitHub server. Response Status: " + response.StatusCode, response.ErrorException);
        }

        internal Task<IEnumerable<GitHubRepository>> GetUserRepositoriesAsync()
        {
            var request = new RestRequest("/user/repos");

            return ExecuteAsync<IEnumerable<GitHubRepository>>(request);
        }

        internal Task<GitHubWorkflowsResponse> GetWorkflowsAsync(string repository)
        {
            var request = new RestRequest($"/repos/{repository}/actions/workflows");

            return ExecuteAsync<GitHubWorkflowsResponse>(request);
        }

        internal Task<GitHubWorkflowRunsResponse> GetWorkflowRunsAsync(string repository, string workflow, string branch)
        {
            var request = new RestRequest($"/repos/{repository}/actions/workflows/{workflow}/runs");

            request.AddQueryParameter("per_page", "1");

            if (!string.IsNullOrEmpty(branch))
            {
                request.AddQueryParameter("branch", branch);
            }

            return ExecuteAsync<GitHubWorkflowRunsResponse>(request);
        }
    }
}
