using AnyStatus.API.Common;
using AnyStatus.Plugins.Azure.API.Contracts;
using AutoMapper;

namespace AnyStatus.Plugins.Azure.DevOps.PullRequests
{
    internal class AzureDevOpsPullRequestSynchronizer : CollectionSynchronizer<GitPullRequest, AzureDevOpsPullRequestWidget>
    {
        public AzureDevOpsPullRequestSynchronizer(IMapper mapper, AzureDevOpsPullRequestsWidget parent)
        {
            Compare = (src, widget) => src.PullRequestId.Equals(widget.PullRequestId);

            Remove = src => parent.Remove(src);

            Update = (src, widget) => mapper.Map(src, widget);

            Add = src => parent.Add(mapper.Map<AzureDevOpsPullRequestWidget>(src));
        }
    }
}
