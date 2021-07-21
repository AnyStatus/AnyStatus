using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class MoveDown<T> : ContextMenu<T> where T : IWidget, IMovable
    {
        public MoveDown()
        {
            Order = 250;
            Name = "Move Down";
            InputGestureText = "Alt+Down";
            Command = new Command(_ => Context.MoveDown());
        }

        public override bool IsEnabled => Context.CanMoveDown();
    }
}
