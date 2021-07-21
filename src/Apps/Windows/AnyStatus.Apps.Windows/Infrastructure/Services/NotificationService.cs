using AnyStatus.API.Notifications;
using AnyStatus.Apps.Windows.Features.SystemTray;

namespace AnyStatus.Apps.Windows.Features.Notifications
{
    internal class NotificationService : INotificationService
    {
        private readonly ISystemTray _systemTray;

        public NotificationService(ISystemTray systemTray) => _systemTray = systemTray;

        public void Send(Notification notification) => _systemTray.ShowNotification(notification);
    }
}
