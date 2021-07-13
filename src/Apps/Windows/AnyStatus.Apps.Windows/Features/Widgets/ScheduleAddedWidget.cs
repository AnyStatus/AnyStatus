using AnyStatus.API.Events;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class ScheduleAddedWidget : INotificationHandler<WidgetAddedNotification>
    {
        private readonly IMediator _mediator;

        public ScheduleAddedWidget(IMediator mediator) => _mediator = mediator;

        public Task Handle(WidgetAddedNotification notification, CancellationToken cancellationToken)
            => _mediator.Send(new ScheduleJob.Request(notification.Widget), cancellationToken);
    }
}
