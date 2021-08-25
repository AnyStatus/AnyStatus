using AnyStatus.API.Widgets;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AnyStatus.Plugins.Azure.WorkItems
{
    [Browsable(false)]
    public class AzureDevOpsWorkItemWidget : Widget, IWebPage
    {
        [ReadOnly(true)]
        [DisplayName("Work Item Id")]
        public string WorkItemId { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public string URL { get; set; }
    }
}
