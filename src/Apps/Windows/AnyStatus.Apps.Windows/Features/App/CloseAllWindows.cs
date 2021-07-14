using AnyStatus.API.Services;
using MediatR;
using System.Windows;

namespace AnyStatus.Apps.Windows.Features.App
{
    public class CloseAllWindows
    {
        public class Request : IRequest { }

        public class Handler : RequestHandler<Request>
        {
            private readonly IDispatcher _dispatcher;

            public Handler(IDispatcher dispatcher) => _dispatcher = dispatcher;

            protected override void Handle(Request request) => _dispatcher.Invoke(Close);

            private static void Close()
            {
                foreach (Window window in Application.Current.Windows)
                {
                    window.Close();
                }
            }
        }
    }
}
