using AnyStatus.API.Widgets;
using MediatR;
using System;

namespace AnyStatus.API.Events
{
    public class WidgetDeletedNotification : INotification
    {
        public WidgetDeletedNotification(IWidget widget)
        {
            Widget = widget ?? throw new ArgumentNullException(nameof(widget));
        }

        public IWidget Widget { get; }
    }
}
