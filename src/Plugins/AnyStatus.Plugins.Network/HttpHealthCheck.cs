using AnyStatus.API.Widgets;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.HealthChecks
{
    public class HttpHealthCheck : AsyncStatusCheck<HttpHealthCheckWidget>
    {
        protected async override Task Handle(StatusRequest<HttpHealthCheckWidget> request, CancellationToken cancellationToken)
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = request.Context.UseDefaultCredentials
            };

            if (request.Context.IgnoreSslErrors)
            {
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }

            try
            {
                using (var client = new HttpClient(handler, true))
                {
                    var response = await client.GetAsync(request.Context.URL).ConfigureAwait(false);

                    request.Context.Status = response.StatusCode == request.Context.HttpStatusCode ? Status.OK : Status.Failed;
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is HttpRequestException)
                    {
                        request.Context.Status = Status.Failed;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
    }
}
