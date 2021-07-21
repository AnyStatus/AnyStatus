﻿using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu
{
    public interface IContextMenu
    {
        string Name { get; }
        string Icon { get; }
        int Order { get; }
        bool IsVisible { get; }
        bool Break { get; }
        string InputGestureText { get; }
        ICommand Command { get; }
    }
}
