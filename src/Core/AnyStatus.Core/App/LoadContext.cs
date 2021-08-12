using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Endpoints;
using AnyStatus.Core.Sessions;
using AnyStatus.Core.Settings;
using AnyStatus.Core.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.App
{
    public sealed class LoadContext
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public Response(IAppContext context) => Context = context;

            public IAppContext Context { get; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger _logger;
            private readonly IMediator _mediator;
            private readonly IAppContext _context;

            public Handler(IMediator mediator, ILogger logger, IAppContext context)
            {
                _logger = logger;
                _context = context;
                _mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Loading session...");

                await Task.WhenAll(LoadUserSettingsAsync(), LoadSessionAsync(), LoadEndpointsAsync());

                _logger.LogInformation("Session loaded.");

                await _mediator.Publish(new ContextLoaded(_context));

                return new Response(_context);
            }

            private async Task LoadEndpointsAsync()
            {
                var response = await _mediator.Send(new GetEndpoints.Request());

                if (response.Success)
                {
                    _context.Endpoints = new ObservableCollection<IEndpoint>(response.Endpoints);

                    return;
                }

                _logger.LogInformation("Initializing endpoints...");

                _context.Endpoints = new ObservableCollection<IEndpoint>();

                await _mediator.Send(new SaveEndpoints.Request());
            }

            private async Task LoadSessionAsync()
            {
                var response = await _mediator.Send(new GetSession.Request());

                if (response.Success)
                {
                    _context.Session = response.Session;

                    return;
                }

                _logger.LogInformation("Initializing session...");

                _context.Session = new Session
                {
                    Widget = new Root()
                };

                await _mediator.Send(new SaveSession.Request());
            }

            private async Task LoadUserSettingsAsync()
            {
                var response = await _mediator.Send(new GetUserSettings.Request());

                if (response.Success)
                {
                    _context.UserSettings = response.UserSettings;

                    return;
                }

                _logger.LogInformation("Initializing user settings...");

                _context.UserSettings = new UserSettings();

                await _mediator.Send(new SaveUserSettings.Request());
            }
        }
    }
}
