using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Settings;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Options
{
    internal class OptionsViewModel : BaseViewModel
    {
        public OptionsViewModel(IMediator mediator, IAppContext context, IPropertyGridViewModel propertyGridViewModel)
        {
            PropertyGridViewModel = propertyGridViewModel;

            Commands.Add("Save", new Command(async _ =>
            {
                var success = await mediator.Send(new SaveUserSettings.Request());

                if (success)
                {
                    //todo: skip if not changed
                    await mediator.Send(new SwitchTheme.Request(context.UserSettings.Theme));

                    await mediator.Send(new ClosePage.Request());
                }
            }));

            Commands.Add("Cancel", new Command(_ => mediator.Send(new ClosePage.Request())));

            PropertyGridViewModel.Target = context.UserSettings;
        }

        public IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}
