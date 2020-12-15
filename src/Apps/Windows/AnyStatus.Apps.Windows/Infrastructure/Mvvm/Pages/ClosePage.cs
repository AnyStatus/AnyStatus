using MediatR;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages
{
    class ClosePage
    {
        internal class Request : IRequest
        {
        }

        internal class Handler : RequestHandler<Request>
        {
            private readonly PagesViewModel _pagesViewModel;

            public Handler(PagesViewModel pagesViewModel)
            {
                _pagesViewModel = pagesViewModel;
            }

            protected override void Handle(Request request)
            {
                _pagesViewModel.Close();
            }
        }
    }
}
