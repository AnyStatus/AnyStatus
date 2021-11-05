using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// NuGet API Documentation
// https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource

namespace AnyStatus.Plugins.NuGet.API
{
    internal class NuGetApi
    {
        private readonly RestClient _client;
        private readonly string _packageSource;

        internal NuGetApi(string packageSource)
        {
            if (string.IsNullOrWhiteSpace(packageSource))
            {
                throw new ArgumentException("NuGet package source cannot be empty.");
            }

            _packageSource = packageSource;

            _client = new RestClient();
        }

        internal async Task<NuGetMetadata> GetPackageMetadataAsync(string packageId, bool prerelease, CancellationToken cancellationToken)
        {
            var resource = await GetResourceAsync("SearchQueryService", cancellationToken).ConfigureAwait(false);

            var packagesMetadata = await GetPackagesMetadataAsync(resource, packageId, prerelease, cancellationToken).ConfigureAwait(false);

            var packageMetadata = packagesMetadata.FirstOrDefault(m => string.Equals(m.Id, packageId, StringComparison.InvariantCultureIgnoreCase));

            return packageMetadata ?? throw new Exception("NuGet package not found.");
        }

        internal async Task<NuGetResource> GetResourceAsync(string name, CancellationToken cancellationToken)
        {
            var resources = await GetResourcesAsync(cancellationToken).ConfigureAwait(false);

            var resource = resources.FirstOrDefault(r => r.Name == name);

            if (resource is null)
                throw new ApplicationException($"Resource name ${name} was not found.");

            return resource;
        }

        internal async Task<IEnumerable<NuGetResource>> GetResourcesAsync(CancellationToken cancellationToken)
        {
            var request = new RestRequest(_packageSource);

            var response = await _client.ExecuteAsync<NuGetIndex>(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful || response.Data is null)
                throw new Exception("An error occurred while getting NuGet resources.");

            return response.Data.Resources;
        }

        internal async Task<IEnumerable<NuGetMetadata>> GetPackagesMetadataAsync(NuGetResource searchQueryService, string packageId, bool prerelease, CancellationToken cancellationToken)
        {
            var metadataRequest = new RestRequest(searchQueryService.URL);

            metadataRequest.AddParameter("semVerLevel", "2.0.0");

            if (prerelease)
            {
                metadataRequest.AddParameter("prerelease", prerelease);
            }

            metadataRequest.AddParameter("q", $"packageid:{packageId}");

            var metadataResponse = await _client.ExecuteAsync<NuGetMetadataCollection>(metadataRequest, cancellationToken).ConfigureAwait(false);

            if (!metadataResponse.IsSuccessful || metadataResponse.Data is null)
            {
                throw new Exception($"An error occurred while getting NuGet package metadata. Response: {metadataResponse.StatusDescription}");
            }

            return metadataResponse.Data.Data;
        }
    }
}
