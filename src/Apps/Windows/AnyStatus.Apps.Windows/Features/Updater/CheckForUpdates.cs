using AnyStatus.Core.Pipeline;
using MediatR;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Updater
{
    [Obsolete("Updates are automatic on Microsoft Store.")]
    public class CheckForUpdates
    {
        public class Request : ITransientRequest
        {
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly ILogger _logger;

            public Handler(ILogger logger) => _logger = logger;

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Checking for updates...");

                var response = await GetVersion(cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Available Version: " + response.Version);
            }

            private async Task<VersionResponse> GetVersion(CancellationToken cancellationToken)
            {
                var client = new RestClient("https://anystatuspublicapi.azurewebsites.net");

                var request = new RestRequest("api/version", Method.POST);

                var response = await client.ExecuteAsync<VersionResponse>(request, cancellationToken).ConfigureAwait(false);

                return response.IsSuccessful
                    ? response.Data
                    : throw new Exception("An error occurred while checking for updates.", response.ErrorException);
            }
        }
    }
}
