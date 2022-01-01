namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class GitPullRequest
    {
        public string PullRequestId { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string MergeStatus { get; set; }

        public string GetStatus() => MergeStatus switch
        {
            "succeeded" => AnyStatus.API.Widgets.Status.OK,
            "queued" => AnyStatus.API.Widgets.Status.Queued,
            "failure" => AnyStatus.API.Widgets.Status.Failed,
            "conflicts" => AnyStatus.API.Widgets.Status.Rejected,
            "rejectedByPolicy" => AnyStatus.API.Widgets.Status.Rejected,
            "notSet" => AnyStatus.API.Widgets.Status.None,
            _ => AnyStatus.API.Widgets.Status.Unknown,
        };
    }
}
