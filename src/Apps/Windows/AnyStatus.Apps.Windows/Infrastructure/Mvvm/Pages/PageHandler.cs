using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    internal class PageHandler : RequestHandler<Page>
    {
        private readonly PagesViewModel _pagesViewModel;
        private readonly IServiceProvider _serviceProvider;

        public PageHandler(IServiceProvider serviceProvider, PagesViewModel pagesViewModel)
        {
            _pagesViewModel = pagesViewModel;
            _serviceProvider = serviceProvider;
        }

        protected override void Handle(Page request)
        {
            var viewModel = request.ViewModel ?? _serviceProvider.GetService(request.Type);

            if (viewModel is null)
            {
                throw new Exception($"View model '{request.Type}' was not found.");
            }

            _pagesViewModel.Add(new PageViewModel(_pagesViewModel)
            {
                Title = request.Title,
                Content = viewModel
            });
        }
    }

    internal class PageHandler<T> : RequestHandler<Page<T>> where T : BaseViewModel
    {
        private readonly PagesViewModel _pagesViewModel;
        private readonly IServiceProvider _serviceProvider;

        public PageHandler(IServiceProvider serviceProvider, PagesViewModel pagesViewModel)
        {
            _pagesViewModel = pagesViewModel;
            _serviceProvider = serviceProvider;
        }

        protected override void Handle(Page<T> request)
        {
            var viewModel = request.ViewModel ?? _serviceProvider.GetService(request.Type);

            request.Initializer((T)viewModel);

            if (viewModel is null)
            {
                throw new Exception($"View model '{request.Type}' was not found.");
            }

            _pagesViewModel.Add(new PageViewModel(_pagesViewModel)
            {
                Title = request.Title,
                Content = viewModel
            });
        }
    }
}
