using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Core.App;
using AnyStatus.Core.Domain;
using MediatR;
using System;
using System.IO;

namespace AnyStatus.Apps.Windows.Features.App
{
    public class ContextLoadedHandler : NotificationHandler<ContextLoaded>
    {
        private readonly IMediator _mediator;

        public ContextLoadedHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void Handle(ContextLoaded notification)
        {
            _mediator.Send(new SwitchTheme.Request(notification.Context.UserSettings.Theme));

            LoadSession(notification.Context.Session);
        }

        private void LoadSession(Session session)
        {
            if (string.IsNullOrEmpty(session.FileName) || !File.Exists(session.FileName))
            {
                session.FileName = null;

                session.Widget = new Root();
            }
            else
            {
                _mediator.Send(new OpenSessionCommand.Request(session.FileName)).ConfigureAwait(false);
            }
        }
    }
}
