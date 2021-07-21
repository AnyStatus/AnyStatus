using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using MediatR;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.NamedPipe
{
    public class StartNamedPipeServer
    {
        public class Request : IRequest { }

        public class Handler : IRequestHandler<Request>
        {
            private readonly IMediator _mediator;
            private readonly IDispatcher _dispatcher;

            public Handler(IMediator mediator, IDispatcher dispatcher)
            {
                _mediator = mediator;
                _dispatcher = dispatcher;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                while (true)
                {
                    using var server = new NamedPipeServerStream("{89790288-AE14-4BE1-A2D2-501EBC3F9C9E}");

                    await server.WaitForConnectionAsync();

                    using var reader = new StreamReader(server);
                    
                    if (await reader.ReadLineAsync() == "activate")
                    {
                        _dispatcher.Invoke(() => _mediator.Send(MaterialWindow.Show<AppViewModel>()));
                    }
                }
            }
        }
    }

}
