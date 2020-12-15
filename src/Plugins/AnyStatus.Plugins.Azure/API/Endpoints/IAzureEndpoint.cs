using AnyStatus.API.Endpoints;
using RestSharp.Authenticators;

namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    public interface IAzureEndpoint : IEndpoint
    {
        IAuthenticator GetAuthenticator();
    }
}
