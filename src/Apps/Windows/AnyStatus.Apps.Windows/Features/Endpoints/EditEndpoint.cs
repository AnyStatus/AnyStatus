using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    class EditEndpoint
    {
        internal class Request : IRequest
        {
            public Request(IEndpoint endpoint) => Endpoint = endpoint;

            public IEndpoint Endpoint { get; }
        }

        internal class Handler : AsyncRequestHandler<Request>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly IEndpointViewModel _viewModel;

            public Handler(IMediator mediator, IMapper mapper, IEndpointViewModel viewModel)
            {
                _mapper = mapper;
                _mediator = mediator;
                _viewModel = viewModel;
            }

            protected override Task Handle(Request request, CancellationToken cancellationToken)
            {
                var clone = (IEndpoint)Activator.CreateInstance(request.Endpoint.GetType());

                _viewModel.Endpoint = _mapper.Map(request.Endpoint, clone);

                return _mediator.Send(Page.Show("Edit Endpoint", _viewModel));
            }
        }
    }
}
