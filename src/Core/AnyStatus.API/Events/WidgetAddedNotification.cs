using AnyStatus.API.Widgets;
using MediatR;
using System;

namespace AnyStatus.API.Events
{
    public class WidgetAddedNotification : INotification
    {
        public WidgetAddedNotification(IWidget widget)
        {
            Widget = widget ?? throw new ArgumentNullException(nameof(widget));
        }

        public IWidget Widget { get; }
    }
}
