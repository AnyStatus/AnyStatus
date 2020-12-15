using AnyStatus.API.Widgets.Events;
using AnyStatus.Core.Jobs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class ScheduleAddedWidget : INotificationHandler<WidgetAddedNotification>
    {
        private readonly IMediator _mediator;

        public ScheduleAddedWidget(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(WidgetAddedNotification notification, CancellationToken cancellationToken)
        {
            var request = new ScheduleJob.Request(notification.Widget);

            await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
