using AnyStatus.API.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public interface IStartable : IActionable
    {
        bool CanStart { get; }
    }

    public interface IStart<T> : IRequestHandler<StartRequest<T>> where T : IStartable { }

    public static class StartRequestFactory
    {
        public static StartRequest<T> Create<T>(T context) where T : IStartable
        {
            return new StartRequest<T>(context);
        }
    }

    public class StartRequest<T> : Request<T> where T : IStartable
    {
        public StartRequest(T context) : base(context) { }
    }

    public abstract class AsyncStartRequestHandler<TWidget> : AsyncRequestHandler<StartRequest<TWidget>>, IStart<TWidget>
        where TWidget : IStartable
    {
        protected abstract override Task Handle(StartRequest<TWidget> request, CancellationToken cancellationToken);
    }
}
