using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    internal class Page<T> : Page
    {
        public Action<T> Initializer { get; internal set; }
    }

    internal class Page : IRequest
    {
        protected Page() { }

        public string Title { get; private set; }

        public Type Type { get; private set; }

        public object ViewModel { get; private set; }

        public Action OnClose { get; internal set; }

        public static ClosePage.Request Close() => new();

        public static Page Show<T>(string title)
        {
            return new Page
            {
                Title = title,
                Type = typeof(T)
            };
        }

        public static Page Show<T>(string title, Action<T> initializer = null, Action onClose = null)
        {
            return new Page<T>
            {
                Title = title,
                Type = typeof(T),
                OnClose = onClose,
                Initializer = initializer,
            };
        }

        public static Page Show<T>(string title, T viewModel)
        {
            return new Page
            {
                Title = title,
                Type = typeof(T),
                ViewModel = viewModel
            };
        }

        public static Page Show<T>(string title, T viewModel, Action<T> initializer)
        {
            return new Page<T>
            {
                Title = title,
                Type = typeof(T),
                ViewModel = viewModel,
                Initializer = initializer
            };
        }
    }
}
