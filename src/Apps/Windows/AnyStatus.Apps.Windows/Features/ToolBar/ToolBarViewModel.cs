using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Activity;
using AnyStatus.Apps.Windows.Features.Menu;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Jobs;
using MediatR;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.ToolBar
{
    public class ToolBarViewModel : BaseViewModel
    {
        private bool _isVisible;

        public ToolBarViewModel(IMediator mediator, IAppContext context)
        {
            Commands.Add("ToggleMenu", new Command(_ => ToggleVisibility(), _ => MenuViewModel != null));
            Commands.Add("Refresh", new Command(_ => Task.Run(async () => await mediator.Send(new TriggerJob.Request(context.Session.Widget)))));
            Commands.Add("ExpandAll", new Command(_ => mediator.Send(new ExpandAll.Request())));
            Commands.Add("CollapseAll", new Command(_ => mediator.Send(new CollapseAll.Request())));
            Commands.Add("AddWidget", new Command(_ => mediator.Send(Page.Show<AddWidgetViewModel>("Add Widget", vm => vm.Parent = context.Session.SelectedWidget ?? context.Session.Widget)), _ => context.Session.SelectedWidget is null || context.Session.SelectedWidget is IAddWidget));
            Commands.Add("AddFolder", new Command(_ => mediator.Send(new AddFolder.Request(context.Session.SelectedWidget ?? context.Session.Widget)), _ => context.Session.SelectedWidget is null || context.Session.SelectedWidget is IAddFolder));
            Commands.Add("Activity", new Command(_ => mediator.Send(MaterialWindow.Show<ActivityViewModel>(title: "Activity", width: 800, height: 600))));
        }

        private void ToggleVisibility() => MenuViewModel.IsVisible = !MenuViewModel.IsVisible;

        public MenuViewModel MenuViewModel { get; set; }

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
    }
}
