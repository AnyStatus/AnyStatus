using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Start<TWidget> : ContextMenu<TWidget> where TWidget : IWidget, IStartable
    {
        public Start(IMediator mediator, IDialogService dialogService)
        {
            Order = 130;
            Name = "Start";
            Icon = "MaterialLight.Play";
            Command = new Command(
                async _ =>
                {
                    var dialog = new ConfirmationDialog($"Are you sure you want to start {Context.Name}?");

                    if (await dialogService.ShowDialogAsync(dialog) is DialogResult.Yes)
                    {
                        await mediator.Send(StartRequestFactory.Create(Context));
                    }
                });
        }

        public override bool IsEnabled => Context.CanStart;
    }
}
