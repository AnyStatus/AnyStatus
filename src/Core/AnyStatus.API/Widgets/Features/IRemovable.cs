using AnyStatus.API.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.API.Widgets
{
    public interface IRemovable : IActionable { }

    internal interface IRemove<T> : IRequestHandler<RemoveRequest<T>> where T : IRemovable { }

    public static class RemoveRequestFactory
    {
        public static RemoveRequest<T> Create<T>(T context) where T : IRemovable
        {
            return new RemoveRequest<T>(context);
        }
    }

    public class RemoveRequest<T> : Request<T> where T : IRemovable
    {
        public RemoveRequest(T context) : base(context) { }
    }

    public abstract class AsyncRemoveRequestHandler<TWidget> : AsyncRequestHandler<RemoveRequest<TWidget>>, IRemove<TWidget>
        where TWidget : IRemovable
    {
        protected abstract override Task Handle(RemoveRequest<TWidget> request, CancellationToken cancellationToken);
    }
}
