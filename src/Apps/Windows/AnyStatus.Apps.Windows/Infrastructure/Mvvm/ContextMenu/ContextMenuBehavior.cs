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
        private readonly ContextMenu<TContext>[] _contextMenuItems;

        private static readonly IContextMenu NoActionsAvailable = new DefaultContextMenuItem<object>();

        public ContextMenuBehavior(ContextMenu<TContext>[] contextMenus) => _contextMenuItems = contextMenus;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();

            if (_contextMenuItems.Any())
            {
                foreach (ContextMenu<TContext> contextMenu in from item in _contextMenuItems
                                                              let ctx = item.Context = request.Context
                                                              where item.IsVisible
                                                              orderby item.Order
                                                              select item)
                {
                    AddContextMenuItem(response, contextMenu);
                }
            }
            else
            {
                response.Add(NoActionsAvailable);
            }

            return response;
        }

        private static void AddContextMenuItem(TResponse response, ContextMenu<TContext> contextMenu)
        {
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
    }
}
