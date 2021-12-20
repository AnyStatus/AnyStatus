using AnyStatus.API.Notifications;
using System;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public interface ISystemTray : IDisposable
    {
        string Status { get; set; }

        void ShowNotification(Notification notification);
    }
}