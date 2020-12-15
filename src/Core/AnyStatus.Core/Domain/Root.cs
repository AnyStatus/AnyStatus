using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.API.Widgets.Events;
using System.ComponentModel;

namespace AnyStatus.Core.Domain
{
    [Browsable(false)]
    [Redirect("AnyStatus.Core.Entities.Root, AnyStatus.Core")]
    public class Root : Widget, IRefreshable, IAddWidget, IAddFolder
    {
        public Root() => Name = "Root";

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName.Equals(nameof(Status)) && Status != PreviousStatus)
            {
                WidgetNotifications.PublishAsync(StatusChangedNotification.Create(this));
            }
        }
    }
}
