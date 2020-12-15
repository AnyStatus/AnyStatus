using MediatR;

namespace AnyStatus.API.Common
{
    public abstract class Request<TContext, TResponse> : IRequest<TResponse>
    {
        protected Request(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; }
    }

    /// <summary>
    /// A general-purpose object for representing a request. 
    /// </summary>
    public abstract class Request<TContext> : Request<TContext, Unit>, IRequest
    {
        protected Request(TContext context) : base(context)
        {
        }
    }
}
