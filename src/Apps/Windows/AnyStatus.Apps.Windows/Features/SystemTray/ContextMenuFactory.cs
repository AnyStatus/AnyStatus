using AnyStatus.Apps.Windows.Features.Activity;
using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Windows.Forms;

namespace AnyStatus.Apps.Windows.Features.SystemTray
{
    public class ContextMenuFactory
    {
        private readonly IAppContext _context;
        private readonly IMediator _mediator;

        public ContextMenuFactory(IMediator mediator, IAppContext context)
        {
            _context = context;
            _mediator = mediator;
        }

        public ContextMenuStrip Create()
        {
            return new ContextMenuStrip
            {
                Items =
                 {
                     new ToolStripMenuItem("Show",null,(s, e) => _mediator.Send(MaterialWindow.Show<AppViewModel>(width: 398,minWidth: 398,height: 418,minHeight: 418))),
                     new ToolStripSeparator(),
                     new ToolStripMenuItem("Refresh",null,(s, e) => _mediator.Send(new TriggerJob.Request(_context.Session.Widget))),
                     new ToolStripMenuItem("Activity",null, (s, e) => _mediator.Send(MaterialWindow.Show<ActivityViewModel>("Activity", 1280, 800))),
                     new ToolStripSeparator(),
                     new ToolStripMenuItem("Exit",null, (s, e) => _mediator.Send(new Shutdown.Request())),
                 }
            };
        }
    }
}
