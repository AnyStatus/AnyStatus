using AnyStatus.Apps.Windows.Features.App;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Features.Launchers;
using AnyStatus.Apps.Windows.Features.Options;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        public MenuViewModel(IMediator mediator)
        {
            Commands.Add("New", new Command(_ => mediator.Send(new NewSessionCommand.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Open", new Command(_ => mediator.Send(new OpenSessionCommand.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Save", new Command(_ => mediator.Send(new SaveCommand.Request()).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("SaveAs", new Command(_ => mediator.Send(new SaveCommand.Request(showDialog: true)).ContinueWith(task => IsVisible = !task.Result)));
            Commands.Add("Options", new Command(_ => mediator.Send(Page.Show<OptionsViewModel>("Options")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Endpoints", new Command(_ => mediator.Send(Page.Show<EndpointsViewModel>("Endpoints")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Feedback", new Command(_ => mediator.Send(new LaunchURL.Request(@"https://www.anystat.us/feedback")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Help", new Command(_ => mediator.Send(new LaunchURL.Request(@"https://www.anystat.us/docs")).ContinueWith(task => IsVisible = false)));
            Commands.Add("Exit", new Command(_ => mediator.Send(new Shutdown.Request())));
        }
    }
}
