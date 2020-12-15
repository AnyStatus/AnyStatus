using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class EndpointsViewModel : BaseViewModel
    {
        private readonly IMediator _mediator;
        private EndpointTypeDescription _selectedEndpointTypeDescription;

        public EndpointsViewModel(IMediator mediator)
        {
            _mediator = mediator;

            Initialize();
        }

        private void Initialize()
        {
            ConfigureCommands();

            var typesResponse = _mediator.Send(new GetEndpointTypes.Request()).GetAwaiter().GetResult();

            EndpointTypes = typesResponse.Types;

            var endpointsResponse = _mediator.Send(new GetEndpoints.Request()).GetAwaiter().GetResult();

            Endpoints = endpointsResponse.Endpoints;
        }

        public Endpoint SelectedEndpoint { get; set; }

        public ObservableCollection<IEndpoint> Endpoints { get; private set; }

        public IEnumerable<EndpointTypeDescription> EndpointTypes { get; private set; }

        public EndpointTypeDescription SelectedEndpointTypeDescription
        {
            get => _selectedEndpointTypeDescription;
            set => Set(ref _selectedEndpointTypeDescription, value);
        }

        private void ConfigureCommands()
        {
            Commands.Add("Edit", new Command(p => _mediator.Send(new EditEndpoint.Request((IEndpoint)p)), p => p is IEndpoint));

            Commands.Add("Delete", new Command(p => _mediator.Send(new DeleteEndpoint.Request((IEndpoint)p)), p => p is IEndpoint));

            Commands.Add("Add", new Command(_ => _mediator.Send(new AddEndpoint.Request { Type = SelectedEndpointTypeDescription.Type }), _ => SelectedEndpointTypeDescription is object));
        }
    }
}
