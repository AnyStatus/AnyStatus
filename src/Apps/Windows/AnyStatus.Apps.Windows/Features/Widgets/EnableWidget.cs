using AnyStatus.API.Widgets;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class EnableWidget
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
                request.Widget.IsEnabled = true;

                request.Widget.Parent?.Reassessment();
            }
        }
    }
}
