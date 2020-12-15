namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public interface INotificationService
    {
        void SendNotification(string title, string message);
    }
}
