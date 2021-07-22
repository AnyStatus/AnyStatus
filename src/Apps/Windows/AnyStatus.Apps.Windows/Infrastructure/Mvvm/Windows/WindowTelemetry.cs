using AnyStatus.Core.Telemetry;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    internal class WindowTelemetry : IRequestPostProcessor<MaterialWindow, Unit>
    {
        private readonly ITelemetry _telemetry;

        public WindowTelemetry(ITelemetry telemetry) => _telemetry = telemetry;

        public Task Process(MaterialWindow request, Unit response, CancellationToken cancellationToken)
        {
            _telemetry.TrackView(request.Type.ToFriendlyName());

            return Task.CompletedTask;
        }
    }
}
