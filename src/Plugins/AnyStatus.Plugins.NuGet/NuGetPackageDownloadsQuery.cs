using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.NuGet.API;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.NuGet
{
    public class NuGetPackageDownloadsQuery : AsyncMetricQuery<NuGetPackageDownloadsWidget>, IEndpointHandler<NuGetEndpoint>
    {
        public NuGetEndpoint Endpoint { get; set; }

        protected override async Task Handle(MetricRequest<NuGetPackageDownloadsWidget> request, CancellationToken cancellationToken)
        {
            var api = new NuGetApi(Endpoint.Address);

            var packageMetadata = await api.GetPackageMetadataAsync(request.Context.PackageId, request.Context.PreRelease, cancellationToken).ConfigureAwait(false);

            request.Context.Value = packageMetadata.TotalDownloads;

            request.Context.Status = Status.OK;
        }
    }
}
