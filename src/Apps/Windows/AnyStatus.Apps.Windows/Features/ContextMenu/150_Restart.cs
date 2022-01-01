using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Restart<TWidget> : ContextMenu<TWidget> where TWidget : IWidget, IRestartable
    {
        public Restart(IMediator mediator, IDialogService dialogService)
        {
            Order = 150;
            Name = "Restart";
            Icon = "Material.Reload";
            Command = new Command(async _ =>
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to restart {Context.Name}?");

                if (await dialogService.ShowDialogAsync(dialog) is DialogResult.Yes)
                {
                    await mediator.Send(RestartRequestFactory.Create(Context));
                }
            });
        }
    }
}
