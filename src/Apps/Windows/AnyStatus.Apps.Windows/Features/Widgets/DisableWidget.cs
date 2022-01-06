using AnyStatus.API.Widgets;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class DisableWidget
    {
        public class Request : IRequest
        {
            public Request(IWidget widget) => Widget = widget;

            public IWidget Widget { get; }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request)
            {
                Disable(request.Widget);
            }

            private static void Disable(IWidget widget)
            {
                widget.Status = Status.None;

                widget.IsEnabled = false;

                foreach (var child in widget)
                {
                    Disable(child);
                }
            }
        }
    }
}
