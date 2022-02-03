using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.App;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class EndpointsViewModel : BaseViewModel
    {
        private readonly IMediator _mediator;
        private readonly IAppContext _context;
        private EndpointTypeDescription _selectedEndpointTypeDescription;

        public EndpointsViewModel(IMediator mediator, IAppContext context)
        {
            _context = context;
            _mediator = mediator;

            ConfigureCommands();

            Endpoints = _context.Endpoints;

            _ = LoadEndpointTypesAsync();
        }

        private async Task LoadEndpointTypesAsync()
        {
            var response = await _mediator.Send(new GetEndpointTypes.Request());

            if (response is not null)
            {
                EndpointTypes = response.Types;
            }
        }

        private void ConfigureCommands()
        {
            Commands.Add("Edit", new Command(p => _mediator.Send(new EditEndpoint.Request((IEndpoint)p)), p => p is IEndpoint));
            Commands.Add("Delete", new Command(p => _mediator.Send(new DeleteEndpoint.Request((IEndpoint)p)), p => p is IEndpoint));
            Commands.Add("Add", new Command(_ => _mediator.Send(new AddEndpoint.Request { Type = SelectedEndpointTypeDescription.Type }), _ => SelectedEndpointTypeDescription is not null));
        }

        public Endpoint SelectedEndpoint { get; set; }

        public ObservableCollection<IEndpoint> Endpoints { get; private set; }

        public IEnumerable<EndpointTypeDescription> EndpointTypes { get; private set; }

        public EndpointTypeDescription SelectedEndpointTypeDescription
        {
            get => _selectedEndpointTypeDescription;
            set => Set(ref _selectedEndpointTypeDescription, value);
        }
    }
}
