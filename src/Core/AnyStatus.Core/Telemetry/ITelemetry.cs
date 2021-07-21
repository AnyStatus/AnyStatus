using System;
using System.Collections.Generic;

namespace AnyStatus.Core.Telemetry
{
    public interface ITelemetry
    {
        void Enable();

        void Disable();

        void TrackView(string name);

        void TrackView(string name, TimeSpan duration);

        void TrackEvent(string name);

        void TrackEvent(string name, IDictionary<string, string> properties);

        void TrackException(Exception exception);
    }
}
