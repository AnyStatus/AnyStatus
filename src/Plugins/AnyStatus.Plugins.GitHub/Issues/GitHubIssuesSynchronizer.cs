using AnyStatus.API.Common;
using AnyStatus.Plugins.GitHub.API.Models;
using AutoMapper;

namespace AnyStatus.Plugins.GitHub.Issues
{
    internal class GitHubIssuesSynchronizer : CollectionSynchronizer<GitHubIssue, GitHubIssueWidget>
    {
        public GitHubIssuesSynchronizer(IMapper mapper, GitHubIssuesWidget parent)
        {
            Compare = (src, widget) => src.Number == widget.Number;

            Remove = src => parent.Remove(src);

            Update = (src, widget) => mapper.Map(src, widget);

            Add = src => parent.Add(mapper.Map<GitHubIssueWidget>(src));
        }
    }
}
