using System.Collections.Generic;

namespace AnyStatus.Plugins.GitHub.API.Models
{
    class GitHubWorkflowsResponse
    {
        public IEnumerable<GitHubWorkflow> Workflows { get; set; }
    }
}
