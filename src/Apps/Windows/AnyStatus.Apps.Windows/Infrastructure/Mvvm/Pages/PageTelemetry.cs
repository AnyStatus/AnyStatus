using AnyStatus.Core.Services;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    internal class PageTelemetry : IRequestPostProcessor<Page, Unit>
    {
        private readonly ITelemetry _telemetry;

        public PageTelemetry(ITelemetry telemetry)
        {
            _telemetry = telemetry;
        }

        public Task Process(Page request, Unit response, CancellationToken cancellationToken)
        {
            _telemetry.TrackView(request.Type.ToFriendlyName());

            return Task.CompletedTask;
        }
    }
}
