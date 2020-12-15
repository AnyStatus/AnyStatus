using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Navigation;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    public class OAuthEndpointViewModel : BaseViewModel
    {
        private readonly IMediator _mediator;

        public OAuthEndpointViewModel(IMediator mediator) => _mediator = mediator;

        public OAuthEndpoint Endpoint { get; set; }

        public void HandleBrowserNavigation(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(Endpoint?.CallbackURL)) return;

            if (args is NavigatingCancelEventArgs navigation && navigation.Uri.ToString().StartsWith(Endpoint.CallbackURL))
            {
                navigation.Cancel = true;

                var code = ParseCallback(navigation.Uri);

                IBaseRequest request = Endpoint.GrantType switch
                {
                    OAuthGrantTypes.AuthorizationCode => new GetOAuthAccessToken.Request(Endpoint, code),
                    OAuthGrantTypes.JsonWebToken => new GetJwtAccessToken.Request(Endpoint, code),
                    _ => throw new NotSupportedException()
                };

                _mediator.Send(new GetOAuthAccessToken.Request(Endpoint, code))
                         .ContinueWith(task => Save(task.Result), TaskScheduler.FromCurrentSynchronizationContext());

                _mediator.Send(new ClosePage.Request());
            }
        }

        private string ParseCallback(Uri uri)
        {
            var query = HttpUtility.ParseQueryString(uri.Query);

            var error = query.Get("error");

            if (error is object)
            {
                throw new Exception("A remote error occurred while authorizing the token request: " + error);
            }

            var state = query.Get("state");

            if (state != Endpoint.Id)
            {
                throw new SecurityException("A security error occurred. The remote state and local state are different.");
            }

            var code = query.Get("code");

            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("Authentication code is null or empty.");
            }

            return code;
        }

        private void Save(GetOAuthAccessToken.Response response)
        {
            if (response is null || !response.Success) return;

            Endpoint.AccessToken = response.AccessToken;

            Endpoint.RefreshToken = response.RefreshToken;

            _mediator.Send(new SaveEndpoint.Request(Endpoint));
        }
    }
}