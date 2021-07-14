using System.Collections.ObjectModel;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    public class PagesViewModel : BaseViewModel
    {
        private readonly static ObservableCollection<PageViewModel> _pages = new ObservableCollection<PageViewModel>();

        public ObservableCollection<PageViewModel> Pages => _pages;

        internal void Add(PageViewModel viewModel) => _pages.Add(viewModel);

        internal void Close(PageViewModel viewModel) => _pages.Remove(viewModel);

        internal void CloseLastPage() => _pages.RemoveAt(_pages.Count - 1);
    }
}
