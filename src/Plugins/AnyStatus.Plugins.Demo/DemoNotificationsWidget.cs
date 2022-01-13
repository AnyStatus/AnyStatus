using AnyStatus.API.Notifications;
using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.Demo
{
    [Category("AnyStatus")]
    [DisplayName("Notifications Demo")]
    [Description("An example for sending desktop notifications from widgets.")]
    public class DemoNotificationsWidget : TextWidget, IPollable, ICommonWidget
    {
    }

    public class DemoNotificationsHandler : StatusCheck<DemoNotificationsWidget>
    {
        private readonly INotificationService _notificationService;

        public DemoNotificationsHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected override void Handle(StatusRequest<DemoNotificationsWidget> request)
        {
            _notificationService.Send(new Notification("Message", "Title", NotificationIcon.Info));

            request.Context.Status = Status.OK;
        }
    }
}
