using AnyStatus.API.Widgets;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Jobs
{
    public class UnscheduleJob
    {
        public class Request : IRequest
        {
            public Request(IWidget widget) => Widget = widget;

            public IWidget Widget { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IJobScheduler _jobScheduler;

            public Handler(IJobScheduler jobScheduler) => _jobScheduler = jobScheduler;

            protected override Task Handle(Request request, CancellationToken cancellationToken) => Unschedule(request.Widget, cancellationToken);

            private async Task Unschedule(IWidget widget, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    await _jobScheduler.DeleteJobAsync(widget.Id, cancellationToken);
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Unschedule(child, cancellationToken);
                    }
                }
            }
        }
    }
}
