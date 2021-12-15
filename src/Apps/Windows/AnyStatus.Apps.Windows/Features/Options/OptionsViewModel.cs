using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.App;
using AnyStatus.Core.Settings;
using AnyStatus.Core.Telemetry;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Options
{
    internal class OptionsViewModel : BaseViewModel
    {
        public OptionsViewModel(
            IMediator mediator,
            IAppContext context,
            IPropertyGridViewModel propertyGridViewModel,
            ITelemetry telemetry)
        {
            PropertyGridViewModel = propertyGridViewModel;

            PropertyGridViewModel.Target = context.UserSettings;

            Commands.Add("Save", new Command(async _ =>
            {
                if (await mediator.Send(new SaveUserSettings.Request()))
                {
                    if (context.UserSettings.SendAnonymousUsageStatistics)
                    {
                        telemetry.Enable();
                    }
                    else
                    {
                        telemetry.Disable();
                    }

                    _ = await mediator.Send(new SwitchTheme.Request(context.UserSettings.Theme)); //todo: skip if not changed

                    _ = await mediator.Send(Page.Close());
                }
            }));

            Commands.Add("Cancel", new Command(_ => mediator.Send(Page.Close())));
        }

        public IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}
