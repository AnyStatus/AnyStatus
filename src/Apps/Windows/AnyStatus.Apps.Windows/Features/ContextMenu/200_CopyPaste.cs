using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class CopyPaste
    {
        private static IWidget _clone;

        public class Copy<T> : ContextMenu<T> where T : class, IWidget, ICopyable
        {
            public Copy()
            {
                Order = 200;
                Name = "Copy";
                Icon = "Material.ContentCopy";
                Command = new Command(_ => _clone = (IWidget)Context.Clone());
            }
        }

        public class Paste<T> : ContextMenu<T> where T : class, IWidget, IAddWidget
        {
            public Paste()
            {
                Order = 201;
                Name = "Paste";
                Icon = "Material.ContentPaste";
                Command = new Command(_ =>
                {
                    Context.Add(_clone);
                    Context.Expand();
                    _clone = null;
                });
            }

            public override bool IsEnabled => _clone != null;
        }
    }
}
