using AnyStatus.API.Common;
using AnyStatus.API.Notifications;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Infrastructure.Converters;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.App;
using Hardcodet.Wpf.TaskbarNotification;
using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using MediatR;
using System.Windows.Media.Imaging;
//using System.Windows.Forms;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public sealed class SystemTray : NotifyPropertyChanged, ISystemTray
    {
        private bool _disposed;
        private string _status;
        private readonly IMediator _mediator;
        //private readonly NotifyIcon _notifier;
        //private readonly ContextMenuStrip _contextMenu;
        private readonly TaskbarIcon _taskbarIcon;

        public SystemTray(IMediator mediator, IAppContext context)
        {
            _mediator = mediator;

            _taskbarIcon = new TaskbarIcon
            {
                ToolTipText = "AnyStatus",
                LeftClickCommand = new Command(_ => mediator.Send(MaterialWindow.Show<AppViewModel>())),
                Icon = SystemTrayIcons.Get(Status.OK)
            };

            //_contextMenu = new ContextMenuFactory(mediator, context).Create();

            //_notifier = new NotifyIcon
            //{
            //    Visible = true,
            //    Text = "AnyStatus",
            //    ContextMenuStrip = _contextMenu,
            //    Icon = SystemTrayIcons.Get(API.Widgets.Status.OK)
            //};

            //WireEvents();
        }

        //public string Status
        //{
        //    get => _status;
        //    set => Set(ref _status, value);
        //}

        //private void WireEvents()
        //{
        //    PropertyChanged += SetIcon;
        //    _notifier.MouseClick += OnMouseClick;
        //    SystemEvents.DisplaySettingsChanged += SetIcon;
        //}

        //private void UnWireEvents()
        //{
        //    PropertyChanged -= SetIcon;
        //    _notifier.MouseClick -= OnMouseClick;
        //    SystemEvents.DisplaySettingsChanged -= SetIcon;
        //}

        public void ShowStatus(string status)
        {
            _taskbarIcon.Icon = SystemTrayIcons.Get(status);
        }

        public void ShowNotification(Notification notification)
        {
            //const string DefaultTitle = "AnyStatus";

            //if (notification is null)
            //{
            //    throw new ArgumentNullException(nameof(notification));
            //}

            //var icon = notification.Icon switch
            //{
            //    NotificationIcon.Info => ToolTipIcon.Info,
            //    NotificationIcon.Error => ToolTipIcon.Error,
            //    NotificationIcon.Warning => ToolTipIcon.Warning,
            //    _ => ToolTipIcon.None,
            //};

            //_notifier.ShowBalloonTip(1000, string.IsNullOrEmpty(notification.Title) ? DefaultTitle : notification.Title, notification.Message, icon);
        }

        //private void OnMouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        _mediator.Send(MaterialWindow.Show<AppViewModel>(width: 398, minWidth: 398, height: 418, minHeight: 418));
        //    }
        //}

        public void Dispose()
        {
            _taskbarIcon.Dispose();
        }
    }
}
