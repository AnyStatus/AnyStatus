namespace AnyStatus.API.Notifications
{
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        public Notification(string message, string title)
        {
            Title = title;
            Message = message;
        }

        public Notification(string message, string title, NotificationIcon icon)
        {
            Icon = icon;
            Title = title;
            Message = message;
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationIcon Icon { get; set; }
    }
}
