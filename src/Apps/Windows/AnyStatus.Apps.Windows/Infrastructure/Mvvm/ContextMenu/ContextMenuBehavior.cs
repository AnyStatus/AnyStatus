using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu
{
    internal class ContextMenuBehavior<TRequest, TResponse, TContext> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : DynamicContextMenu.Request<TContext>
            where TResponse : ICollection<IContextMenu>
    {
        private readonly ContextMenu<TContext>[] _contextMenus;
        private static readonly IContextMenu Default = new DefaultContextMenuItem<TContext>();

        public ContextMenuBehavior(ContextMenu<TContext>[] contextMenus) => _contextMenus = contextMenus;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (_contextMenus.Length == 0)
            {
                response.Add(Default);

                return response;
            }

            foreach (var contextMenu in _contextMenus.OrderBy(k => k.Order))
            {
                contextMenu.Context = request.Context;

                if (!contextMenu.IsVisible)
                {
                    continue;
                }

                if (contextMenu.IsSeparator)
                {
                    response.Add(null);
                }
                else
                {
                    response.Add(contextMenu);

                    if (contextMenu.Break)
                    {
                        response.Add(null);
                    }
                }
            }

            return response;
        }
    }
}
