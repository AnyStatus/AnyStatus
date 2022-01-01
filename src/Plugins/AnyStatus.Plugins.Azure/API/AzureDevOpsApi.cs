using AnyStatus.API.Endpoints;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.API.Endpoints;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Azure.API
{
    internal class AzureDevOpsApi
    {
        private readonly IRestClient _client;
        private readonly IAzureDevOpsEndpoint _endpoint;

        internal AzureDevOpsApi(IAzureDevOpsEndpoint endpoint)
        {
            _endpoint = endpoint ?? throw new EndpointNotFoundException();

            _client = new RestClient(endpoint.Address)
            {
                Authenticator = endpoint.GetAuthenticator()
            };
        }

        private async Task<T> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken) where T : new()
        {
            if (_endpoint is AzureDevOpsEndpoint)
            {
                request.AddParameter("api-version", "5.1");
            }

            request.AddHeader("X-TFS-FedAuthRedirect", "Suppress");

            var response = await _client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful || response.ErrorException != null)
            {
                throw new Exception("An error occurred while requesting data from Azure DevOps. " + response.StatusDescription, response.ErrorException);
            }

            return response.Data;
        }

        private async Task ExecuteAsync(IRestRequest request, CancellationToken cancellationToken)
        {
            if (_endpoint is AzureDevOpsEndpoint)
            {
                request.AddParameter("api-version", "5.1", ParameterType.QueryString);
            }

            request.AddHeader("X-TFS-FedAuthRedirect", "Suppress");

            var response = await _client.ExecuteAsync(request, request.Method, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful || response.ErrorException != null)
            {
                throw new Exception($"An error occurred while sending request to Azure DevOps. {response.StatusDescription}.", response.ErrorException);
            }
        }

        //Profile

        internal Task<Profile> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("https://app.vssps.visualstudio.com/_apis/profile/profiles/me", Method.GET);

            return ExecuteAsync<Profile>(request, cancellationToken);
        }

        //Projects

        internal Task<CollectionResponse<Project>> GetProjectsAsync(string organization, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(Uri.EscapeDataString(organization) + "/_apis/projects", Method.GET);

            return ExecuteAsync<CollectionResponse<Project>>(request, cancellationToken);
        }

        //Collections

        internal Task<CollectionResponse<ProjectCollection>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("_apis/projectcollections/");

            return ExecuteAsync<CollectionResponse<ProjectCollection>>(request, cancellationToken);
        }

        //Accounts

        internal Task<CollectionResponse<Account>> GetAccountsAsync(string memberId, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("https://app.vssps.visualstudio.com/_apis/accounts", Method.GET);

            request.AddParameter("memberId", memberId);

            return ExecuteAsync<CollectionResponse<Account>>(request, cancellationToken);
        }

        //Pipelines

        internal Task<CollectionResponse<Build>> GetBuildsAsync(string organization, string project, string definitionId, int top, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/build/builds", Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            request.AddParameter("$top", top);
            request.AddParameter("queryOrder","queueTimeDescending");
            request.AddParameter("definitions", definitionId);

            return ExecuteAsync<CollectionResponse<Build>>(request, cancellationToken);
        }

        internal Task<CollectionResponse<BuildDefinition>> GetBuildDefinitionsAsync(string organization, string project, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/build/definitions", Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            return ExecuteAsync<CollectionResponse<BuildDefinition>>(request, cancellationToken);
        }

        internal Task QueueBuildAsync(string organization, string project, string definitionId, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/build/builds", Uri.EscapeDataString(organization), Uri.EscapeDataString(project)), Method.POST);

            request.AddJsonBody(new { Definition = new { Id = definitionId } });

            return ExecuteAsync(request, cancellationToken);
        }

        internal Task CancelBuildAsync(string organization, string project, string buildId, CancellationToken cancellationToken)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/build/builds/{2}", Uri.EscapeDataString(organization), Uri.EscapeDataString(project), Uri.EscapeDataString(buildId)), Method.PATCH);

            request.AddJsonBody(new { status = "cancelling" });

            return ExecuteAsync(request, cancellationToken);
        }

        //Releases

        internal Task<CollectionResponse<ReleaseDefinition>> GetReleaseDefinitionsAsync(string organization, string project, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/release/definitions", _endpoint.ReleaseManagement, Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            return ExecuteAsync<CollectionResponse<ReleaseDefinition>>(request, cancellationToken);
        }

        internal Task<CollectionResponse<Release>> GetReleasesAsync(string organization, string project, int definitionId, int top, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/release/releases", _endpoint.ReleaseManagement, Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            request.AddParameter("$top", top);
            request.AddParameter("$expand", "environments");
            request.AddParameter("definitionId", definitionId);

            return ExecuteAsync<CollectionResponse<Release>>(request, cancellationToken);
        }

        internal Task CreateReleaseAsync(string project, int definitionId, CancellationToken cancellationToken)
        {
            var request = new RestRequest(Uri.EscapeDataString(project) + "/_apis/release/releases", Method.POST);

            request.AddJsonBody(new { definitionId });

            return ExecuteAsync(request, cancellationToken);
        }

        //Deployments

        internal Task<CollectionResponse<Deployment>> GetDeploymentsAsync(string project, int definitionId, int top, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(Uri.EscapeDataString(project) + "/_apis/release/deployments");

            request.AddParameter("$top", top);
            request.AddParameter("definitionId", definitionId);

            return ExecuteAsync<CollectionResponse<Deployment>>(request, cancellationToken);
        }

        internal Task<CollectionResponse<Deployment>> GetDeploymentsAsync(string project, int definitionId, int definitionEnvironmentId, int top, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(Uri.EscapeDataString(project) + "/_apis/release/deployments");

            request.AddParameter("$top", top);
            request.AddParameter("definitionId", definitionId);
            request.AddParameter("definitionEnvironmentId", definitionEnvironmentId);

            return ExecuteAsync<CollectionResponse<Deployment>>(request, cancellationToken);
        }

        internal Task DeployAsync(string project, int releaseId, int deploymentId, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"{Uri.EscapeDataString(project)}/_apis/release/releases/{releaseId}/environments/{deploymentId}", Method.PATCH);

            request.AddJsonBody(new { status = "inProgress" });

            return ExecuteAsync(request, cancellationToken);
        }

        internal Task CancelDeploymentAsync(string project, int releaseId, int deploymentId, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"{Uri.EscapeDataString(project)}/_apis/release/releases/{releaseId}/environments/{deploymentId}", Method.PATCH);

            request.AddJsonBody(new { status = "canceled" });

            return ExecuteAsync(request, cancellationToken);
        }

        //Work Items

        internal Task<WorkItemQueryResult> QueryWorkItemsAsync(string organization, string project, string query, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/wit/wiql?api-version=5.0", Uri.EscapeDataString(organization), Uri.EscapeDataString(project)), Method.POST);

            request.AddJsonBody(new { query });

            return ExecuteAsync<WorkItemQueryResult>(request, cancellationToken);
        }

        internal Task<WorkItemQueryResult> QueryWorkItemsByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest($"_apis/wit/wiql/{id}?api-version=5.0");

            return ExecuteAsync<WorkItemQueryResult>(request, cancellationToken);
        }

        internal Task<CollectionResponse<WorkItem>> GetWorkItemsAsync(string organization, string project, List<string> ids, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/_apis/wit/workitemsbatch?api-version=5.0", Uri.EscapeDataString(organization), Uri.EscapeDataString(project)), Method.POST);

            request.AddJsonBody(new Dictionary<string, object>
            {
                ["ids"] = ids,
                ["$expand"] = "Links",
                ["fields"] = new[] { "System.Id", "System.Title", "System.WorkItemType", "System.TeamProject" }
            });

            return ExecuteAsync<CollectionResponse<WorkItem>>(request, cancellationToken);
        }

        internal Task<CollectionResponse<WorkItemQuery>> GetWorkItemQueriesAsync(string project, string filter, int top, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(Uri.EscapeDataString(project) + "/_apis/wit/queries");

            request.AddParameter("$top", top);
            request.AddParameter("$filter", Uri.EscapeDataString(filter));

            return ExecuteAsync<CollectionResponse<WorkItemQuery>>(request, cancellationToken);
        }

        //Repositories

        internal Task<CollectionResponse<GitRepository>> GetRepositoriesAsync(string organization, string project, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/git/repositories", _endpoint.Address, Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            return ExecuteAsync<CollectionResponse<GitRepository>>(request, cancellationToken);
        }

        internal Task<GitRepository> GetRepositoryAsync(string organization, string project, string name, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/git/repositories/{3}", _endpoint.Address, Uri.EscapeDataString(organization), Uri.EscapeDataString(project), Uri.EscapeDataString(name)));

            return ExecuteAsync<GitRepository>(request, cancellationToken);
        }

        //Pull Requests

        internal Task<CollectionResponse<GitPullRequest>> GetPullRequestsAsync(string organization, string project, string status = "active", CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/git/pullrequests", _endpoint.Address, Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            request.AddParameter("searchCriteria.status", status);
            
            //request.AddParameter("searchCriteria.includeLinks", true); //web link is not included in the response

            return ExecuteAsync<CollectionResponse<GitPullRequest>>(request, cancellationToken);
        }
    
        //Iterations
        internal Task<CollectionResponse<Iteration>> GetIterationsAsync(string organization, string project, CancellationToken cancellationToken = default)
        {
            var request = new RestRequest(string.Format("{0}/{1}/{2}/_apis/work/teamsettings/iterations", _endpoint.ReleaseManagement, Uri.EscapeDataString(organization), Uri.EscapeDataString(project)));

            return ExecuteAsync<CollectionResponse<Iteration>>(request, cancellationToken);
        }
    }
}
