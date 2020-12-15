using AnyStatus.API.Common;
using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Pipeline.Decorators
{
    public sealed class EndpointHandlerDecorator<TRequest, TResponse, TContext, TEndpoint> : IRequestHandler<TRequest, TResponse>
        where TRequest : Request<TContext>, IRequest<TResponse>
        where TContext : IRequireEndpoint<TEndpoint>
        where TEndpoint : IEndpoint
    {
        private readonly IAppContext _context;
        private readonly IRequestHandler<TRequest, TResponse> _handler;

        public EndpointHandlerDecorator(IRequestHandler<TRequest, TResponse> handler, IAppContext context)
        {
            _handler = handler;
            _context = context;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (_handler is IEndpointHandler<TEndpoint> endpointHandler)
            {
                if (string.IsNullOrEmpty(request.Context.EndpointId))
                {
                    throw new InvalidOperationException("Endpoint not configured.");
                }

                endpointHandler.Endpoint = _context.Endpoints.OfType<TEndpoint>().FirstOrDefault(endpoint => endpoint.Id == request.Context.EndpointId);

                if (endpointHandler.Endpoint is null)
                {
                    throw new InvalidOperationException("Endpoint not found.");
                }
            }

            return _handler.Handle(request, cancellationToken);
        }
    }
}