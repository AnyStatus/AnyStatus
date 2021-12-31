using AnyStatus.API.Widgets;
using System.ComponentModel;

namespace AnyStatus.Plugins.GitHub.Issues
{
    [Browsable(false)]
    public class GitHubIssueWidget : Widget, IWebPage
    {
        public string Number { get; set; }

        public string URL { get; set; }
    }
}
