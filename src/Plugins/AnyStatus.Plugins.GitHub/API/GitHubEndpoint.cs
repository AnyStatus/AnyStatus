using AnyStatus.API.Endpoints;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.GitHub.API
{
    [DisplayName("GitHub")]
    public class GitHubEndpoint : OAuthEndpoint
    {
        public GitHubEndpoint()
        {
            Name = "GitHub";
            Scope = "read:org%20repo";
            ClientId = "e13245dc0d76e568dbe5";
            Address = "https://api.github.com";
            GrantType = OAuthGrantTypes.AuthorizationCode;
            CallbackURL = "https://anystat.us/oauth/callback";
            Secret = "b491fb955f1c49b3911ab5759267d86cde7db919";
            AuthorizeURL = "https://github.com/login/oauth/authorize";
            TokenURL = "https://github.com/login/oauth/access_token";
        }

        [JsonIgnore]
        [Browsable(false)]
        public override string AuthorizeURL
        {
            get => $"{base.AuthorizeURL}?client_id={ClientId}&state={Id}&scope={Scope}&allow_signup=false&redirect_uri={CallbackURL}";
            set => base.AuthorizeURL = value;
        }

        [Browsable(false)]
        public IAuthenticator GetAuthenticator() => new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
    }
}
