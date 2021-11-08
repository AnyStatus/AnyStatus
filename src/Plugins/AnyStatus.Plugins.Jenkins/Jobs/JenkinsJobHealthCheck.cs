using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Jenkins.API;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Jenkins.Jobs
{
    public class JenkinsJobHealthCheck : AsyncStatusCheck<JenkinsJobWidget>, IEndpointHandler<JenkinsEndpoint>
    {
        public JenkinsEndpoint Endpoint { get; set; }

        protected override async Task Handle(StatusRequest<JenkinsJobWidget> request, CancellationToken cancellationToken)
        {
            var api = new JenkinsApi(Endpoint);

            var job = await api.GetJobAsync(request.Context.Job, cancellationToken).ConfigureAwait(false);

            request.Context.Status = job is null ? Status.Unknown : job.Status;
        }
    }
}
