namespace AnyStatus.Plugins.GitHub.API.Models
{
    internal class GitHubIssue
    {
        public string Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string HtmlUrl { get; set; }
    }
}
