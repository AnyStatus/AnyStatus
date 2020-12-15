using Newtonsoft.Json;
using System.ComponentModel;

namespace AnyStatus.API.Endpoints
{
    public enum OAuthGrantTypes
    {
        None,
        JsonWebToken,
        AuthorizationCode,
    }

    public abstract class OAuthEndpoint : Endpoint
    {
        [JsonIgnore]
        [Browsable(false)]
        public OAuthGrantTypes GrantType { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string ClientId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string Secret { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string Scope { get; set; }

        [DisplayName("Access Token")]
        public string AccessToken { get; set; }

        [DisplayName("Refresh Token")]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string TokenURL { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string CallbackURL { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual string AuthorizeURL { get; set; }
    }
}
