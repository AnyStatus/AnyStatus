using AnyStatus.API.Endpoints;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.GitHub
{
    [DisplayName("GitHub")]
    public class GitHubEndpoint : OAuthEndpoint
    {
        public GitHubEndpoint()
        {
            Name = "GitHub";
            GrantType = OAuthGrantTypes.AuthorizationCode;
            ClientId = "e13245dc0d76e568dbe5";
            CallbackURL = "https://anystat.us/oauth/callback";
            Secret = "46ce6d45e777e61ffd730d70b8d5ba9c6c165d09";
            AuthorizeURL = "https://github.com/login/oauth/authorize";
            TokenURL = "https://github.com/login/oauth/access_token";
            Scope = "(no scope)";
        }

        [JsonIgnore]
        [Browsable(false)]
        public override string AuthorizeURL
        {
            get => $"{base.AuthorizeURL}?client_id={ClientId}&response_type=Assertion&state={Id}&scope={Scope}&redirect_uri={CallbackURL}";
            set => base.AuthorizeURL = value;
        }

        [Browsable(false)]
        public IAuthenticator GetAuthenticator() => new JwtAuthenticator(AccessToken);
    }
}
