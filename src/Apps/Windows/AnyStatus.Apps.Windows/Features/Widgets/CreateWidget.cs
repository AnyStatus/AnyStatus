using AnyStatus.API.Common;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.App;
using AnyStatus.Core.Widgets;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class CreateWidget
    {
        public class Request : IRequest
        {
            public Request(Template template, IWidget parent)
            {
                Parent = parent;
                Template = template;
            }

            public IWidget Parent { get; }

            public Template Template { get; set; }
        }

        public class Validator : IValidator<Request>
        {
            public IEnumerable<ValidationResult> Validate(Request request)
            {
                if (request.Parent is null)
                {
                    yield return new ValidationResult("Parent is required.");
                }

                if (request.Template is null)
                {
                    yield return new ValidationResult("Template is required.");
                }
            }
        }

        public class Handler : RequestHandler<Request>
        {
            private readonly IMediator _mediator;
            private readonly IAppContext _context;

            public Handler(IAppContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            protected override void Handle(Request request)
            {
                var widget = CreateWidget(request);

                request.Parent.Add(widget);

                request.Parent.IsExpanded = true;

                _context.Session.IsDirty = true;

                _mediator.Send(Page.Close());

                if (widget.IsConfigurable())
                {
                    _context.Session.SelectedWidget = widget;

                    _mediator.Send(Page.Show<WidgetViewModel>("Configure Widget"));
                }
            }

            private static IWidget CreateWidget(Request request)
            {
                var widget = (IWidget)Activator.CreateInstance(request.Template.Type);

                widget.Id = Guid.NewGuid().ToString();

                widget.Name = request.Template.Name;

                return widget;
            }
        }
    }
}
