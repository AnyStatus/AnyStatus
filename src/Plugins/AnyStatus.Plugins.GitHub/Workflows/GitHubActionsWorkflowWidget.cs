using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using AnyStatus.Plugins.GitHub.API.Sources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.GitHub.Workflows
{
    [Category("GitHub")]
    [DisplayName("GitHub Actions Workflow")]
    [Description("View the status of the latest workflow run on GitHub")]
    public class GitHubActionsWorkflowWidget : StatusWidget, IRequireEndpoint<GitHubEndpoint>, IStandardWidget, IPollable
    {
        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        [Refresh(nameof(Repository))]
        public string EndpointId { get; set; }

        [Required]
        [Refresh(nameof(Workflow))]
        [AsyncItemsSource(typeof(GitHubRepositorySource), autoload: true)]
        public string Repository { get; set; }

        [Required]
        [AsyncItemsSource(typeof(GitHubWorkflowSource))]
        public string Workflow { get; set; }

        [Description("Optional branch name")]
        public string Branch { get; set; }
    }
}
