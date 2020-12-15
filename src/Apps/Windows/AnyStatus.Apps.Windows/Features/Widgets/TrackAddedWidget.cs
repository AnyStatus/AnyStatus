using AnyStatus.API.Events;
using AnyStatus.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class TrackAddedWidget : NotificationHandler<WidgetAddedNotification>
    {
        private readonly ITelemetry _telemetry;

        public TrackAddedWidget(ITelemetry telemetry)
        {
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }

        protected override void Handle(WidgetAddedNotification notification)
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

            _telemetry.TrackEvent("Add Widget", properties);
        }
    }
}
