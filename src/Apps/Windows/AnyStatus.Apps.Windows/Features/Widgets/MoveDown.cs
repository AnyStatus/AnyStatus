using System;
using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    [Obsolete]
    public class MoveDown
    {
        public class Request : Request<IWidget>
        {
            public Request(IWidget widget) : base(widget)
            {
            }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request)
            {
                request.Context.MoveDown();
            }
        }
    }
}