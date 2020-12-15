using AnyStatus.API.Widgets;
using System;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public interface ISystemTray : IDisposable
    {
        Status Status { get; set; }
    }
}