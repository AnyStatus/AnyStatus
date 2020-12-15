using AnyStatus.API.Widgets;
using AnyStatus.API.Events;
using AnyStatus.Apps.Windows.Features.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class StatusChangedNotificationHandler<TWidget> : NotificationHandler<StatusChangedNotification<TWidget>>
        where TWidget : class, IWidget
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public StatusChangedNotificationHandler(IMediator mediator, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void Handle(StatusChangedNotification<TWidget> notification)
        {
            var message = $"Widget '{notification.Widget.Name}' status changed from {notification.Widget.PreviousStatus?.Metadata?.DisplayName} to {notification.Widget.Status?.Metadata?.DisplayName}.";

            _logger.LogDebug(message);

            if (notification.Widget.NotificationsSettings?.IsEnabled == true && notification.Widget.PreviousStatus != Status.None)
            {
                _mediator.Send(new Notification.Request(notification.Widget.Name, message));
            }
        }
    }
}
