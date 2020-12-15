using AnyStatus.API.Endpoints;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    [DisplayName(DefaultName)]
    public class AzureDevOpsEndpoint : Endpoint, IAzureDevOpsEndpoint
    {
        public const string DefaultName = "Azure DevOps";

        public AzureDevOpsEndpoint()
        {
            Name = DefaultName;
            Address = "https://dev.azure.com";
            ReleaseManagement = "https://vsrm.dev.azure.com";
        }

        [DisplayName("Personal Access Token")]
        public string PersonalAccessToken { get; set; }

        [Browsable(false)]
        public string ReleaseManagement { get; set; }

        public IAuthenticator GetAuthenticator() => new HttpBasicAuthenticator(null, PersonalAccessToken);
    }
}
