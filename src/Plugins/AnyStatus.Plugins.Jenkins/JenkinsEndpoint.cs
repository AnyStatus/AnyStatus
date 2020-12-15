using AnyStatus.API.Endpoints;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.Jenkins
{
    [Browsable(true)]
    [DisplayName("Jenkins")]
    public class JenkinsEndpoint : Endpoint
    {
        [DisplayName("Authentication Type")]
        public EndpointAuthenticationType AuthenticationType { get; set; }

        [DisplayName("Personal Access Token")]
        public string PersonalAccessToken { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [Description("Enable if the \"Prevent Cross Site Request Forgery exploits\" security option is enabled on the Jenkins server.")]
        public bool CSRF { get; set; }

        [DisplayName("Ignore SSL Errors")]
        public bool IgnoreSslErrors { get; set; }

        public IAuthenticator GetAuthenticator() => AuthenticationType switch
        {
            EndpointAuthenticationType.Basic => new HttpBasicAuthenticator(UserName, Password),
            EndpointAuthenticationType.PersonalAccessToken => new HttpBasicAuthenticator(string.Empty, PersonalAccessToken),
            _ => null
        };
    }
}
