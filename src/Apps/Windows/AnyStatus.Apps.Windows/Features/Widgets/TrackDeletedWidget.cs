using AnyStatus.API.Widgets.Events;
using AnyStatus.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class TrackDeletedWidget : NotificationHandler<WidgetDeletedNotification>
    {
        private readonly ITelemetry _telemetry;

        public TrackDeletedWidget(ITelemetry telemetry)
        {
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }

        protected override void Handle(WidgetDeletedNotification notification)
        {
            var type = notification.Widget.GetType();
            var name = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name;
            var category = type.GetCustomAttribute<CategoryAttribute>()?.Category ?? "Unknown";

            var properties = new Dictionary<string, string>
                {
                    { "Name", name},
                    { "Category",  category},
                    { "Type", type.FullName },
                };

            _telemetry.TrackEvent("Delete Widget", properties);
        }
    }
}
