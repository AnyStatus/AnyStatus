using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class EndpointViewModel : BaseViewModel, IEndpointViewModel
    {
        private IEndpoint _endpoint;

        public EndpointViewModel(IMediator mediator, IPropertyGridViewModel propertyGridViewModel)
        {
            PropertyGridViewModel = propertyGridViewModel;

            Commands.Add("Save", new Command(async _ =>
            {
                var success = await mediator.Send(new SaveEndpoint.Request(Endpoint));

                if (success)
                {
                    _ = mediator.Send(Page.Close());
                }
            }));

            Commands.Add("Cancel", new Command(_ => mediator.Send(Page.Close())));
        }

        public IEndpoint Endpoint
        {
            get => _endpoint;
            set => PropertyGridViewModel.Target = _endpoint = value;
        }

        public IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}
