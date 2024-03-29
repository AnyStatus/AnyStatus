﻿using AnyStatus.Apps.Windows.Features.Themes;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.App;
using AnyStatus.Core.Features;
using AnyStatus.Core.Telemetry;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Settings
{
    internal class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(
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

                    await mediator.Send(new ChangeTheme.Request(context.UserSettings.Theme)); //todo: skip if not changed

                    await mediator.Send(Page.Close());
                }
            }));

            Commands.Add("Cancel", new Command(_ => mediator.Send(Page.Close())));
        }

        public IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}
