using AnyStatus.API.Events;
using AnyStatus.API.Notifications;
using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class StatusChangedNotificationHandler<TWidget> : NotificationHandler<StatusChangedNotification<TWidget>>
        where TWidget : class, IWidget
    {
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;

        public StatusChangedNotificationHandler(INotificationService notificationService, ILogger logger)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        protected override void Handle(StatusChangedNotification<TWidget> notification)
        {
            var message = $"Widget '{notification.Widget.Name}' status changed from {notification.Widget.PreviousStatus?.Metadata?.DisplayName} to {notification.Widget.Status?.Metadata?.DisplayName}.";

            _logger.LogDebug(message);

            if (notification.Widget.NotificationsSettings?.IsEnabled == true && notification.Widget.PreviousStatus != Status.None)
            {
                _notificationService.Send(new Notification(message, notification.Widget.Name));
            }
        }
    }
}
