using AnyStatus.API.Endpoints;
using MediatR;
using RestSharp;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class GetOAuthAccessToken
    {
        public class Request : IRequest<Response>
        {
            public Request(OAuthEndpoint endpoint, string code)
            {
                Code = code;
                Endpoint = endpoint;
            }

            public string Code { get; set; }

            public OAuthEndpoint Endpoint { get; }
        }

        public class Response
        {
            public bool Success { get; set; }

            public string AccessToken { get; set; }

            public string RefreshToken { get; set; }
        }

        public class Handler : RequestHandler<Request, Response>
        {
            protected override Response Handle(Request request)
            {
                var client = new RestClient(request.Endpoint.TokenURL);

                var restRequest = new RestRequest(Method.POST)
                    .AddHeader("Content-Type", "application/x-www-form-urlencoded")
                    .AddParameter("code", request.Code)
                    .AddParameter("grant_type", "authorization_code")
                    .AddParameter("client_id", request.Endpoint.ClientId)
                    .AddParameter("client_secret", request.Endpoint.Secret)
                    .AddParameter("redirect_uri", request.Endpoint.CallbackURL);

                var result = client.Execute<OAuthAuthorizationResponse>(restRequest);

                var response = new Response();

                if (result.IsSuccessful)
                {
                    response.Success = true;
                    response.AccessToken = result.Data.AccessToken;
                    response.RefreshToken = result.Data.RefreshToken;
                }
                else
                {
                    response.Success = false;
                }

                return response;
            }
        }
    }
}
