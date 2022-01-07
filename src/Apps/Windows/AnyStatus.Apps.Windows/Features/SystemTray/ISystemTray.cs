using AnyStatus.API.Notifications;
using System;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public interface ISystemTray : IDisposable
    {
        void ShowStatus(string status);

        void ShowNotification(Notification notification);
    }
}