using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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
            public Response(IAppContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));

            public IAppContext Context { get; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger _logger;
            private readonly IAppContext _context;
            private readonly IMediator _mediator;

            public Handler(IMediator mediator, ILogger logger, IAppContext context)
            {
                _logger = logger;
                _context = context;
                _mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Loading session...");

                await Task.WhenAll(
                        LoadUserSettingsAsync(cancellationToken),
                        LoadSessionAsync(cancellationToken),
                        LoadEndpointsAsync(cancellationToken))
                            .ConfigureAwait(false);

                _logger.LogInformation("Session loaded.");

                await _mediator.Publish(new ContextLoaded(_context), cancellationToken).ConfigureAwait(false);

                return new Response(_context);
            }

            private async Task LoadEndpointsAsync(CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetEndpoints.Request(), cancellationToken).ConfigureAwait(false);

                if (response.Success)
                {
                    if (response.Endpoints != null)
                    {
                        _context.Endpoints = new ObservableCollection<IEndpoint>(response.Endpoints);

                        return;
                    }
                }

                _logger.LogInformation("Initializing endpoints...");

                _context.Endpoints = new ObservableCollection<IEndpoint>();

                await _mediator.Send(new SaveEndpoints.Request(), cancellationToken).ConfigureAwait(false);
            }

            private async Task LoadSessionAsync(CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetSession.Request(), cancellationToken).ConfigureAwait(false);

                if (response.Success)
                {
                    _context.Session = response.Session;

                    return;
                }

                _logger.LogInformation("Initializing session...");

                _context.Session = new Session
                {
                    Widget = new Root
                    {
                        NotificationsSettings = new WidgetNotificationSettings()
                    }
                };

                await _mediator.Send(new SaveSession.Request(), cancellationToken).ConfigureAwait(false);
            }

            private async Task LoadUserSettingsAsync(CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetUserSettings.Request(), cancellationToken).ConfigureAwait(false);

                if (response.Success)
                {
                    _context.UserSettings = response.UserSettings;

                    return;
                }

                _logger.LogInformation("Initializing user settings...");

                _context.UserSettings = new UserSettings();

                await _mediator.Send(new SaveUserSettings.Request(), cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
