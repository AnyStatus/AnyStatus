using AnyStatus.API.Endpoints;
using SimpleInjector;
using System;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class EndpointViewModelFactory : IEndpointViewModelFactory
    {
        private readonly Container _container;

        public EndpointViewModelFactory(Container container) => _container = container;

        public object Create(Type type)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type);
            
            endpoint.Id = Guid.NewGuid().ToString();

            if (endpoint is OAuthEndpoint oauthEndpoint)
            {
                var viewModel = _container.GetInstance<OAuthEndpointViewModel>();

                viewModel.Endpoint = oauthEndpoint;

                return viewModel;
            }
            else
            {
                var viewModel = _container.GetInstance<IEndpointViewModel>();

                viewModel.Endpoint = endpoint;

                return viewModel;
            }
        }
    }
}
