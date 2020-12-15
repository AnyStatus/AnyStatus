using AnyStatus.Core.Domain;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnyStatus.Core.Services
{
    public sealed class AppInsightsTelemetry : ITelemetry, IDisposable
    {
        private readonly TelemetryClient _client;

        public AppInsightsTelemetry(IAppSettings settings)
        {
            var config = new TelemetryConfiguration(settings.InstrumentationKey)
            {
                TelemetryChannel =
                {
                    DeveloperMode = Debugger.IsAttached
                }
            };

            _client = new TelemetryClient(config);
            _client.Context.User.Id = new UserIdFactory().Create();
            _client.Context.Session.Id = Guid.NewGuid().ToString();
            _client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            _client.Context.Component.Version = GetType().Assembly.GetName().Version.ToString();
        }

        public void Dispose()
        {
            _client.Flush();
        }

        public void TrackView(string name)
        {
            _client.TrackPageView(name);
        }

        public void TrackView(string name, TimeSpan duration)
        {
            _client.TrackPageView(new PageViewTelemetry
            {
                Name = name,
                Duration = duration,
                Timestamp = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
            });
        }

        public void TrackEvent(string name)
        {
            _client.TrackEvent(name);
        }

        public void TrackEvent(string name, IDictionary<string, string> properties)
        {
            _client.TrackEvent(name, properties);
        }

        public void TrackException(Exception exception)
        {
            _client.TrackException(exception);
        }
    }
}
