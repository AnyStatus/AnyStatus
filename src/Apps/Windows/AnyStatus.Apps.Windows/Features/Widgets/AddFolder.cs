using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class AddFolder
    {
        public class Request : IRequest
        {
            public Request(IWidget parent) => Parent = parent;

            public IWidget Parent { get; }
        }

        public class Validator : IValidator<Request>
        {
            public IEnumerable<ValidationResult> Validate(Request request)
            {
                if (request.Parent is null)
                {
                    yield return new ValidationResult("Parent is required.");
                }
            }
        }

        public class Handler : RequestHandler<Request>
        {
            private readonly IAppContext _context;

            public Handler(IAppContext context) => _context = context;

            protected override void Handle(Request request)
            {
                var folder = new FolderWidget
                {
                    Name = "New Folder",
                    NotificationsSettings = new WidgetNotificationSettings()
                };

                request.Parent.Add(folder);

                request.Parent.IsExpanded = true;

                _context.Session.IsDirty = true;
            }
        }
    }
}