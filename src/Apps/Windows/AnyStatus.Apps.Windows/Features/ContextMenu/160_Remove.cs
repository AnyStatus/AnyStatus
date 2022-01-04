using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Remove<T> : ContextMenu<T> where T : IWidget, IRemovable
    {
        public Remove(IMediator mediator, IDialogService dialogService)
        {
            Order = 220;
            Name = "Remove";
            Icon = "Material.DeleteForeverOutline";
            Command = new Command(async _ =>
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to remove {Context.Name}?");

                if (await dialogService.ShowDialogAsync(dialog) is DialogResult.Yes)
                {
                    await mediator.Send(RemoveRequestFactory.Create(Context));
                }
            });
        }
    }
}
