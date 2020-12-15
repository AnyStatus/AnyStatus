using AnyStatus.API.Endpoints;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    [DisplayName(DisplayName)]
    public class AzureOAuthEndpoint : OAuthEndpoint, IAzureEndpoint
    {
        private const string DisplayName = "Azure";

        public AzureOAuthEndpoint()
        {
            Name = DisplayName;
            Address = @"https://management.azure.com";
            GrantType = OAuthGrantTypes.AuthorizationCode;
            ClientId = "9b4b715b-616e-4165-baf8-f9a52efb1781";
            Scope = "https://management.azure.com/user_impersonation";
            TokenURL = @"https://login.microsoftonline.com/common/oauth2/v2.0/token";
            CallbackURL = @"https://login.microsoftonline.com/common/oauth2/nativeclient";
            AuthorizeURL = @"https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        }

        [JsonIgnore]
        [Browsable(false)]
        public override string AuthorizeURL
        {
            get => $"{base.AuthorizeURL}?client_id={ClientId}&response_type=code&response_mode=query&state={Id}&scope={Scope}&redirect_uri={CallbackURL}";
            set => base.AuthorizeURL = value;
        }

        [Browsable(false)]
        public IAuthenticator GetAuthenticator() => new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
    }
}
