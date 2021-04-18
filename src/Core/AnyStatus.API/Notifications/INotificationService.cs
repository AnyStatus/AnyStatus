namespace AnyStatus.API.Notifications
{
    public interface INotificationService
    {
        void Send(Notification notification);
    }
}
