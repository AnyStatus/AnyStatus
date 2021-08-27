using AnyStatus.API.Common;
using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    class AddEndpoint
    {
        internal class Request : IRequest
        {
            public Type Type { get; set; }
        }

        internal class Validator : IValidator<Request>
        {
            public IEnumerable<ValidationResult> Validate(Request request)
            {
                if (request.Type is null)
                {
                    yield return new ValidationResult("Endpoint type not specified.");
                }
                else if (!typeof(IEndpoint).IsAssignableFrom(request.Type))
                {
                    yield return new ValidationResult($"The type '{request.Type.Name}' does not implement interface IEndpoint.");
                }
            }
        }

        internal class Handler : AsyncRequestHandler<Request>
        {
            private readonly IMediator _mediator;
            private readonly IEndpointViewModelFactory _viewModelFactory;

            public Handler(IMediator mediator, IEndpointViewModelFactory viewModelFactory)
            {
                _mediator = mediator;
                _viewModelFactory = viewModelFactory;
            }

            protected override Task Handle(Request request, CancellationToken cancellationToken)
            {
                var viewModel = _viewModelFactory.Create(request.Type);

                return _mediator.Send(Page.Show("Add Endpoint", viewModel));
            }
        }
    }
}
