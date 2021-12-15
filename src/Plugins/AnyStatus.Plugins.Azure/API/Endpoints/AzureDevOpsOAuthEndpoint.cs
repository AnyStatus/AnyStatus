using AnyStatus.API.Endpoints;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.API.Endpoints
{
    [DisplayName(DisplayName)]
    public class AzureDevOpsOAuthEndpoint : OAuthEndpoint, IAzureDevOpsEndpoint
    {
        private const string DisplayName = "Azure DevOps OAuth";

        public AzureDevOpsOAuthEndpoint()
        {
            Name = DisplayName;
            Address = "https://dev.azure.com";
            GrantType = OAuthGrantTypes.JsonWebToken;
            ReleaseManagement = "https://vsrm.dev.azure.com";
            ClientId = "62719EC7-6746-46DD-82D0-FFF98214CE8F";
            CallbackURL = @"https://anystat.us/azure/devops/callback";
            Scope = "vso.analytics vso.build_execute vso.environment_manage vso.graph vso.identity vso.profile_write vso.project vso.release_execute vso.taskgroups_read vso.test_write vso.work_write";
            Secret = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im9PdmN6NU1fN3AtSGpJS2xGWHo5M3VfVjBabyJ9.eyJjaWQiOiI2MjcxOWVjNy02NzQ2LTQ2ZGQtODJkMC1mZmY5ODIxNGNlOGYiLCJjc2kiOiIxOWZmZDcwZS01MzAxLTRhYTgtYjNlNi1jZDhlMjA0NzBlYzAiLCJuYW1laWQiOiJjYzRjZTIzZC01MjEzLTY1NjctOGZlZS1iNDA4Yzk5NjE3MzgiLCJpc3MiOiJhcHAudnN0b2tlbi52aXN1YWxzdHVkaW8uY29tIiwiYXVkIjoiYXBwLnZzdG9rZW4udmlzdWFsc3R1ZGlvLmNvbSIsIm5iZiI6MTYwMDA3NzQwMCwiZXhwIjoxNzU3ODQzODAwfQ.GoHOg2lQpqHWiNUBiMmVgabUtZXL6Yr7LddKJ11lN19YViOXKpsLayHf6X2vdXge2fvP3LzqwYnlc2PG5aJw0OzRyoaMjYnYtHqimantYjFGYnUlNLsshitbQNfi_Km0KDqTPJQuKeJhvTmER1TY_Ir4BVwKxhZxXHU-uIqLq2790h5iuheoHbOwgbnbzfX0HB4pqzD1teLUKWiIvNWbDnkRHyqQCuXPYS2AoaY06e_cy8tQqxbPvz37SHM5OgKDwyZfdlyj31Ej0eex14pT07BczCZx9OIC8YWOwFvVl69tNgEtIjoCQVM1j9tmuLfBKnZLqzt_hxivC8FcFAD1mA";
            TokenURL = @"https://app.vssps.visualstudio.com/oauth2/token";
            AuthorizeURL = @"https://app.vssps.visualstudio.com/oauth2/authorize";
        }

        [Browsable(false)]
        public string ReleaseManagement { get; }

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
