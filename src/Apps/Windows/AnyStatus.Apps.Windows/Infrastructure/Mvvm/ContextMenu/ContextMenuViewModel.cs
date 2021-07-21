using AnyStatus.API.Common;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Features.ContextMenu
{
    internal class ContextMenuViewModel : NotifyPropertyChanged, IContextMenuViewModel
    {
        private ICollection<IContextMenu> _items;

        public ICollection<IContextMenu> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }
        
        public void Clear()
        {
            Items?.Clear();
            Items = null;
        }
    }
}
