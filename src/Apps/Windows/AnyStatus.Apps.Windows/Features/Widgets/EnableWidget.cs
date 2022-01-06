using AnyStatus.API.Widgets;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class EnableWidget
    {
        public class Request : IRequest
        {
            public Request(IWidget widget) => Widget = widget;

            public IWidget Widget { get; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                Enable(request.Widget);

                EnableParents(request.Widget);

                await _mediator.Send(new Refresh.Request(request.Widget));
            }

            private static void EnableParents(IWidget widget)
            {
                if (widget.Parent is null)
                {
                    return;
                }

                widget.Parent.IsEnabled = true;

                EnableParents(widget.Parent);
            }

            private static void Enable(IWidget widget)
            {
                widget.IsEnabled = true;

                foreach (var child in widget)
                {
                    Enable(child);
                }
            }
        }
    }
}
