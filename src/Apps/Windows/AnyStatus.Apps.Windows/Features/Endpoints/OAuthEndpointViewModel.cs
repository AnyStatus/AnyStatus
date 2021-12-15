using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;
using Microsoft.Web.WebView2.Core;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Web;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    public class OAuthEndpointViewModel : BaseViewModel
    {
        private readonly IMediator _mediator;

        public OAuthEndpointViewModel(IMediator mediator) => _mediator = mediator;

        public OAuthEndpoint Endpoint { get; set; }

        public void HandleBrowserNavigation(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(Endpoint?.CallbackURL))
            {
                return;
            }

            if (args is CoreWebView2NavigationStartingEventArgs navigation && navigation.Uri.StartsWith(Endpoint.CallbackURL))
            {
                navigation.Cancel = true;

                var code = ParseCallback(navigation.Uri);

                IRequest<AccessTokenResponse> request = Endpoint.GrantType switch
                {
                    OAuthGrantTypes.AuthorizationCode => new GetOAuthAccessToken.Request(Endpoint, code),
                    OAuthGrantTypes.JsonWebToken => new GetJwtAccessToken.Request(Endpoint, code),
                    OAuthGrantTypes.None => throw new NotImplementedException(),
                    _ => throw new NotSupportedException()
                };

                _mediator.Send(request).ContinueWith(task => Save(task.Result), TaskScheduler.FromCurrentSynchronizationContext());

                _mediator.Send(new ClosePage.Request());
            }
        }

        private string ParseCallback(string url)
        {
            var uri = new Uri(url);

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

        private void Save(AccessTokenResponse atr)
        {
            if (atr is null)
            {
                return;
            }

            Endpoint.AccessToken = atr.AccessToken;
            
            Endpoint.RefreshToken = atr.RefreshToken;

            _mediator.Send(new SaveEndpoint.Request(Endpoint));
        }
    }
}