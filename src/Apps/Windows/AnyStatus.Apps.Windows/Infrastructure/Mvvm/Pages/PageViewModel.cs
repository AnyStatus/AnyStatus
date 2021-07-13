using System;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    public class PageViewModel : BaseViewModel
    {
        private string _title;
        private object _content;
        private readonly PagesViewModel _pagesViewModel;

        public PageViewModel(PagesViewModel pagesViewModel)
        {
            _pagesViewModel = pagesViewModel;

            Commands.Add("Close", new Command(_ => Close()));
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public object Content
        {
            get => _content;
            set => Set(ref _content, value);
        }
        public Action OnClose { get; internal set; }

        public void Close()
        {
            _pagesViewModel.Close(this);

            if (Content is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Title = null;
            Content = null;

            OnClose?.Invoke();
        }
    }
}
