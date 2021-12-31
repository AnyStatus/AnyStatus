using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.GitHub.Issues
{
    [Category("GitHub")]
    [DisplayName("GitHub Issues")]
    [Description("List issues assigned to the authenticated user across all visible repositories ")]
    public class GitHubIssuesWidget : MetricWidget, IRequireEndpoint<GitHubEndpoint>, IStandardWidget, IPollable
    {
        public GitHubIssuesWidget() => IsPersisted = false;

        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        public string EndpointId { get; set; }
    }
}
