using AnyStatus.API.Endpoints;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.AppVeyor
{
    [DisplayName(DefaultName)]
    public class AppVeyorEndpoint : Endpoint
    {
        public const string DefaultName = "AppVeyor";

        public AppVeyorEndpoint()
        {
            Name = DefaultName;
            Address = "https://ci.appveyor.com"; ;
        }

        [DisplayName("Personal Access Token")]
        public string PersonalAccessToken { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        public IAuthenticator GetAuthenticator() => new JwtAuthenticator(PersonalAccessToken);
    }
}
