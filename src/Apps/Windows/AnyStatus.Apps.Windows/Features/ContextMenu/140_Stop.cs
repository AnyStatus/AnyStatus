using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.ContextMenu;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class Stop<TWidget> : ContextMenu<TWidget> where TWidget : IWidget, IStoppable
    {
        public Stop(IMediator mediator, IDialogService dialogService)
        {
            Order = 140;
            Name = "Stop";
            Icon = "MaterialLight.Stop";
            Command = new Command(async _ =>
            {
                var dialog = new ConfirmationDialog($"Are you sure you want to stop {Context.Name}?");

                if (await dialogService.ShowDialogAsync(dialog) is DialogResult.Yes)
                {
                    await mediator.Send(StopRequestFactory.Create(Context));
                }
            });
        }

        public override bool IsEnabled => Context.CanStop;
    }
}
