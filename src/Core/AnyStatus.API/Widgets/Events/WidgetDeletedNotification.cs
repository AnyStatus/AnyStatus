using MediatR;
using System;

namespace AnyStatus.API.Widgets.Events
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
