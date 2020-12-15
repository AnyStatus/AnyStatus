namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class OAuthAuthorizationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
    }
}