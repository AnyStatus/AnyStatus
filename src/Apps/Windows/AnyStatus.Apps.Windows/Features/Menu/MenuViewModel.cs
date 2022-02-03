using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Features.Help;
using AnyStatus.Apps.Windows.Features.Settings;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.Features;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private bool _isVisible;
        private readonly IMediator _mediator;

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        public MenuViewModel(IMediator mediator)
        {
            _mediator = mediator;

            Commands.Add("New", new Command(async _ => await _mediator.Send(new NewSession.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Open", new Command(async _ => await _mediator.Send(new OpenSession.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Save", new Command(async _ => await _mediator.Send(new Save.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("SaveAs", new Command(async _ => await _mediator.Send(new Save.Request(showDialog: true)).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Settings", new Command(async _ => await _mediator.Send(Page.Show<SettingsViewModel>("Settings")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Endpoints", new Command(async _ => await _mediator.Send(Page.Show<EndpointsViewModel>("Endpoints")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Help", new Command(async _ => await _mediator.Send(Page.Show<HelpViewModel>("Help")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Exit", new Command(async _ => await _mediator.Send(new Shutdown.Request())));
        }
    }
}
