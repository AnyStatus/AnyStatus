using AnyStatus.Core.App;
using AnyStatus.Core.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnyStatus.Core.Telemetry
{
    public sealed class AppInsightsTelemetry : ITelemetry, IDisposable
    {
        private TelemetryClient _client;
        private readonly ILogger _logger;
        private readonly IAppSettings _settings;

        public AppInsightsTelemetry(ILogger logger, IAppSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void Enable()
        {
            if (_client is not null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_settings.InstrumentationKey))
            {
                _logger.LogDebug("Telemetry instrumentation key not found.");

                return;
            }

            var config = new TelemetryConfiguration(_settings.InstrumentationKey) { TelemetryChannel = { DeveloperMode = Debugger.IsAttached } };

            _client = new TelemetryClient(config);
            _client.Context.User.Id = new UserIdFactory().Create();
            _client.Context.Session.Id = Guid.NewGuid().ToString();
            _client.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            _client.Context.Component.Version = GetType().Assembly.GetName().Version.ToString();

            _logger.LogWarning("Telemetry is enabled.");
        }

        public void Disable()
        {
            if (_client is null)
            {
                return;
            }

            _client.Flush();

            _client = null;

            _logger.LogDebug("Telemetry is disabled.");
        }

        public void Dispose() => _client?.Flush();
        public void TrackEvent(string name) => _client?.TrackEvent(name);
        public void TrackView(string name) => _client?.TrackPageView(name);
        public void TrackException(Exception exception) => _client?.TrackException(exception);
        public void TrackEvent(string name, IDictionary<string, string> properties) => _client?.TrackEvent(name, properties);
        public void TrackView(string name, TimeSpan duration) => _client?.TrackPageView(new PageViewTelemetry(name)
        {
            Duration = duration,
            Timestamp = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString(),
        });
    }
}
