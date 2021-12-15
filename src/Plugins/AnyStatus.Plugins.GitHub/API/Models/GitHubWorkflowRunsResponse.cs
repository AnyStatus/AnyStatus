using System.Collections.Generic;

namespace AnyStatus.Plugins.GitHub.API.Models
{
    internal class GitHubWorkflowRunsResponse
    {
        public IEnumerable<GitHubWorkflowRun> WorkflowRuns { get; set; }
    }
}
