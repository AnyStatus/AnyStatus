using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using MediatR;
using System;
using System.Linq;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class DuplicateWidget
    {
        public class Request : Request<IWidget>
        {
            public Request(IWidget widget) : base(widget) { }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request)
            {
                var widget = request.Context;

                if (widget.Parent is null)
                {
                    throw new InvalidOperationException("Parent not found.");
                }

                var clone = (IWidget)widget.Clone();

                clone.Name = GenerateName(widget);

                widget.Parent.Add(clone);
            }

            private static string GenerateName(IWidget item)
            {
                var i = 1;
                string name;

                do name = $"{item.Name} #{i++}";
                while (item.Parent.Any(child => child.Name == name));

                return name;
            }
        }
    }
}