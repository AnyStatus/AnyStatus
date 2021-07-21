using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Features.ContextMenu
{
    public interface IContextMenuViewModel
    {
        void Clear();

        ICollection<IContextMenu> Items { get; set; }
    }
}
