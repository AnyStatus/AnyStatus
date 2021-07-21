using AnyStatus.API.Endpoints;
using AnyStatus.API.Services;
using AnyStatus.Core.App;
using AnyStatus.Core.Endpoints;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class SaveEndpoint
    {
        internal class Request : IRequest<bool>
        {
            public Request()
            {
            }

            public Request(IEndpoint endpoint) => Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            public IEndpoint Endpoint { get; set; }
        }

        internal class Handler : IRequestHandler<Request, bool>
        {
            private readonly IMapper _mapper;
            private readonly IAppContext _context;
            private readonly IMediator _mediator;
            private readonly IDispatcher _dispatcher;

            public Handler(IMediator mediator, IMapper mapper, IAppContext context, IDispatcher dispatcher)
            {
                _mapper = mapper;
                _context = context;
                _mediator = mediator;
                _dispatcher = dispatcher;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Endpoint.Id) || !_context.Endpoints.Any(endpoint => endpoint.Id == request.Endpoint.Id))
                {
                    var endpoint = _mapper.Map<Endpoint>(request.Endpoint);

                    _dispatcher.Invoke(() => _context.Endpoints.Add(endpoint));
                }
                else
                {
                    var endpoint = _context.Endpoints.FirstOrDefault(k => k.Id == request.Endpoint.Id);

                    if (endpoint is null)
                    {
                        throw new InvalidOperationException("Endpoint not found.");
                    }

                    _mapper.Map(request.Endpoint, endpoint);
                }

                await _mediator.Send(new SaveEndpoints.Request());

                return true;
            }
        }
    }
}