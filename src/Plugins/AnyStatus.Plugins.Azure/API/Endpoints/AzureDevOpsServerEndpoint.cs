using AnyStatus.API.Endpoints;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    [DisplayName(DefaultName)]
    public class AzureDevOpsServerEndpoint : Endpoint, IAzureDevOpsEndpoint
    {
        public const string DefaultName = "Azure DevOps Server";

        public AzureDevOpsServerEndpoint()
        {
            Name = DefaultName;
        }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [Browsable(false)]
        public string ReleaseManagement => Address;

        public IAuthenticator GetAuthenticator() => new NtlmAuthenticator(UserName, Password);
    }
}
