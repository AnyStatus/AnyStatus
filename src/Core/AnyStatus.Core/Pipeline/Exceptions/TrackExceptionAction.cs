using System;
using AnyStatus.Core.Services;
using MediatR.Pipeline;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    public class TrackExceptionAction<TRequest> : RequestExceptionAction<TRequest>
    {
        private readonly ITelemetry _telemetry;

        public TrackExceptionAction(ITelemetry telemetry) =>
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));

        protected override void Execute(TRequest request, Exception exception)
        {
            _telemetry.TrackException(exception);
        }
    }
}
