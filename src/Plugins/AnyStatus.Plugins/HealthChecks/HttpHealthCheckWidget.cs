using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AnyStatus.Plugins.HealthChecks
{
    [DisplayName("HTTP")]
    [Category("Health Checks")]
    [Description("Monitor the health of HTTP servers by sending periodic health checks")]
    public class HttpHealthCheckWidget : StatusWidget, IPollable, IWebPage, IStandardWidget
    {
        [Required]
        public string URL { get; set; }

        [DisplayName("HTTP Status Code")]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        [DisplayName("Ignore SSL errors")]
        public bool IgnoreSslErrors { get; set; }

        [DisplayName("Use Default Credentials")]
        public bool UseDefaultCredentials { get; set; }
    }
}
