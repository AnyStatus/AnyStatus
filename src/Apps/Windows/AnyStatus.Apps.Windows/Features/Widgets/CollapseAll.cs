using AnyStatus.Core.Domain;
using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class CollapseAll
    {
        public class Request : IRequest { }

        public class Handler : RequestHandler<Request>
        {
            private readonly IAppContext _context;

            public Handler(IAppContext context)
            {
                _context = context ?? throw new ArgumentNullException();
            }

            protected override void Handle(Request request)
            {
                _context.Session.Widget?.Collapse(true);
            }
        }
    }
}