using AnyStatus.API.Endpoints;
using MediatR;
using RestSharp;
using System.Text;
using System.Web;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class GetJwtAccessToken
    {
        public class Request : IRequest<AccessTokenResponse>
        {
            public Request(OAuthEndpoint endpoint, string code)
            {
                Code = code;
                Endpoint = endpoint;
            }

            public string Code { get; set; }

            public OAuthEndpoint Endpoint { get; }
        }

        public class Handler : RequestHandler<Request, AccessTokenResponse>
        {
            private const string _format = "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}";

            protected override AccessTokenResponse Handle(Request request)
            {
                var body = string.Format(_format, HttpUtility.UrlEncode(request.Endpoint.Secret), HttpUtility.UrlEncode(request.Code), request.Endpoint.CallbackURL);

                var client = new RestClient(request.Endpoint.TokenURL);

                var restRequest = new RestRequest(Method.POST);

                restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                restRequest.AddHeader("Content-Length", new ASCIIEncoding().GetBytes(body).Length.ToString());

                restRequest.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);

                var result = client.Execute<OAuthAuthorizationResponse>(restRequest);

                var response = new AccessTokenResponse();

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
