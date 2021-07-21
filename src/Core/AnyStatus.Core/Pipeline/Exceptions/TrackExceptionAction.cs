using AnyStatus.Core.Telemetry;
using MediatR.Pipeline;
using System;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    public class TrackExceptionAction<TRequest> : RequestExceptionAction<TRequest>
    {
        private readonly ITelemetry _telemetry;

        public TrackExceptionAction(ITelemetry telemetry) => _telemetry = telemetry;

        protected override void Execute(TRequest request, Exception exception) => _telemetry.TrackException(exception);
    }
}
