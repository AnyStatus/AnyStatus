using AnyStatus.Apps.Windows.Features.SystemTray;
using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Features.Notifications
{
    public sealed class Notification
    {
        public class Request : IRequest
        {
            public Request(string title, string message)
            {
                Title = title;
                Message = message;
            }

            public string Title { get; set; }

            public string Message { get; set; }
        }

        public class Handler : RequestHandler<Request>
        {
            private readonly INotificationService _notificationService;

            public Handler(INotificationService notificationService, ISystemTray sysTray)
            {
                _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            }

            protected override void Handle(Request request)
            {
                _notificationService.SendNotification(request.Title, request.Message);
            }
        }
    }
}
