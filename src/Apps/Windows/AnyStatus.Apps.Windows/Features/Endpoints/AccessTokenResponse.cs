namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    class AccessTokenResponse
    {
        public bool Success { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
