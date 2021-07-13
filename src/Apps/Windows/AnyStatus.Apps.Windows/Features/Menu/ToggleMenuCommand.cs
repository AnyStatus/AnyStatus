using MediatR;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public class ToggleMenuCommand
    {
        public class Request : IRequest
        {
        }

        public class Handler : RequestHandler<Request>
        {
            private readonly MenuViewModel _menuViewModel;

            public Handler(MenuViewModel menuViewModel) => _menuViewModel = menuViewModel;

            protected override void Handle(Request request) => _menuViewModel.IsVisible = !_menuViewModel.IsVisible;
        }
    }
}
