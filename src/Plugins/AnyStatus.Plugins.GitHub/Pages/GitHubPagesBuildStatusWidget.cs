using AnyStatus.API.Attributes;
using AnyStatus.API.Widgets;
using AnyStatus.Plugins.GitHub.API;
using AnyStatus.Plugins.GitHub.API.Sources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.Plugins.GitHub.Pages
{
    [Category("GitHub")]
    [DisplayName("GitHub Pages")]
    [Description("View the current status of a GitHub Pages build")]
    public class GitHubPagesBuildStatusWidget : StatusWidget, IRequireEndpoint<GitHubEndpoint>, ICommonWidget, IPollable, IOpenInApp
    {
        [Required]
        [EndpointSource]
        [DisplayName("Endpoint")]
        [Refresh(nameof(Repository))]
        public string EndpointId { get; set; }

        [Required]
        [AsyncItemsSource(typeof(GitHubRepositorySource), autoload: true)]
        public string Repository { get; set; }

        [Browsable(false)]
        public string URL { get; set; }
    }
}
