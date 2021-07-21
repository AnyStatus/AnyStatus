using AnyStatus.Core.App;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class CollapseAll
    {
        public class Request : IRequest { }

        public class Handler : RequestHandler<Request>
        {
            private readonly IAppContext _context;

            public Handler(IAppContext context) => _context = context;

            protected override void Handle(Request request) => _context.Session.Widget?.Collapse(includeChildren: true);
        }
    }
}