using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Core.App;
using AnyStatus.Core.Domain;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.App
{
    public class ContextLoadedHandler : INotificationHandler<ContextLoaded>
    {
        private readonly IMediator _mediator;

        public ContextLoadedHandler(IMediator mediator) => _mediator = mediator;

        public async Task Handle(ContextLoaded notification, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SwitchTheme.Request(notification.Context.UserSettings.Theme));

            await OpenSessionAsync(notification.Context.Session);
        }

        private async Task OpenSessionAsync(Session session)
        {
            if (string.IsNullOrEmpty(session.FileName) || !File.Exists(session.FileName))
            {
                session.FileName = null;
                session.Widget = new Root();
            }
            else
            {
                await _mediator.Send(new OpenSessionCommand.Request(session.FileName));
            }
        }
    }
}
