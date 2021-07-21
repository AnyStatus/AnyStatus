using AnyStatus.Core.App;
using MediatR;

namespace AnyStatus.Core.Telemetry
{
    public class ConfigureTelemetryWhenContextLoaded : NotificationHandler<ContextLoaded>
    {
        private readonly ITelemetry _telemetry;

        public ConfigureTelemetryWhenContextLoaded(ITelemetry telemetry) => _telemetry = telemetry;

        protected override void Handle(ContextLoaded notification)
        {
            if (notification.Context.UserSettings.SendAnonymousUsageStatistics)
            {
                _telemetry.Enable();
            }
            else
            {
                _telemetry.Disable();
            }
        }
    }
}
