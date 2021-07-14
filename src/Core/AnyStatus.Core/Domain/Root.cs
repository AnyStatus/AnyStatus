using AnyStatus.API.Attributes;
using AnyStatus.API.Events;
using AnyStatus.API.Widgets;
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
                _ = WidgetNotifications.PublishAsync(StatusChangedNotification.Create(this));
            }
        }
    }
}
