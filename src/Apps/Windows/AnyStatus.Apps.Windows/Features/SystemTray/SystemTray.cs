using AnyStatus.API.Common;
using AnyStatus.API.Notifications;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.App;
using MediatR;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public sealed class SystemTray : NotifyPropertyChanged, ISystemTray
    {
        private bool _disposed;
        private Status _status = Status.None;
        private readonly IMediator _mediator;
        private readonly NotifyIcon _notifier;
        private readonly ContextMenuStrip _contextMenu;

        public SystemTray(IMediator mediator, IAppContext context)
        {
            _mediator = mediator;

            _contextMenu = new ContextMenuFactory(mediator, context).Create();

            _notifier = new NotifyIcon
            {
                Visible = true,
                Text = "AnyStatus",
                ContextMenuStrip = _contextMenu,
                Icon = SystemTrayIcons.Get(Status.OK)
            };

            WireEvents();
        }

        public Status Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        private void WireEvents()
        {
            PropertyChanged += SetIcon;
            _notifier.MouseClick += OnMouseClick;
            SystemEvents.DisplaySettingsChanged += SetIcon;
        }

        private void UnWireEvents()
        {
            PropertyChanged -= SetIcon;
            _notifier.MouseClick -= OnMouseClick;
            SystemEvents.DisplaySettingsChanged -= SetIcon;
        }

        public void ShowNotification(Notification notification)
        {
            const string DefaultTitle = "AnyStatus";

            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var icon = notification.Icon switch
            {
                NotificationIcon.Info => ToolTipIcon.Info,
                NotificationIcon.Error => ToolTipIcon.Error,
                NotificationIcon.Warning => ToolTipIcon.Warning,
                _ => ToolTipIcon.None,
            };

            _notifier.ShowBalloonTip(1000, string.IsNullOrEmpty(notification.Title) ? DefaultTitle : notification.Title, notification.Message, icon);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mediator.Send(MaterialWindow.Show<AppViewModel>(width: 398, minWidth: 398, height: 418, minHeight: 418));
            }
        }

        private void SetIcon(object sender, EventArgs e) => _notifier.Icon = SystemTrayIcons.Get(Status);

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            UnWireEvents();

            if (_notifier != null)
            {
                _notifier.Icon?.Dispose();
                _notifier.Dispose();
            }

            //_contextMenu.Dispose();

            _disposed = true;
        }
    }
}
