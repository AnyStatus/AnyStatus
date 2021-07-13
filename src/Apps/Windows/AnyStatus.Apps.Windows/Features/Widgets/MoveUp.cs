using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    [Obsolete]
    public class MoveUp
    {
        public class Request : Request<IWidget>
        {
            public Request(IWidget widget) : base(widget)
            {
            }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request) => request.Context.MoveUp();
        }
    }
}