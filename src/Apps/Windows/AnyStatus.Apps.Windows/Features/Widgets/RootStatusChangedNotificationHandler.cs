using AnyStatus.API.Events;
using AnyStatus.Apps.Windows.Features.SystemTray;
using AnyStatus.Core.Domain;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class RootStatusChangedNotificationHandler<TWidget> : NotificationHandler<StatusChangedNotification<TWidget>> where TWidget : Root
    {
        private readonly ISystemTray _sysTray;

        public RootStatusChangedNotificationHandler(ISystemTray sysTray) => _sysTray = sysTray;

        protected override void Handle(StatusChangedNotification<TWidget> notification) => _sysTray.Status = notification?.Widget?.Status;
    }
}
