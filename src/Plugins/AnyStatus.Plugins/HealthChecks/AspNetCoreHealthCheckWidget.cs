using AnyStatus.API.Widgets;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

//https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.diagnostics.healthchecks.healthreport?view=dotnet-plat-ext-3.1

namespace AnyStatus.Plugins.HealthChecks
{
    [Browsable(false)]
    [Category("Health Checks")]
    [DisplayName("ASP.NET Core Health")]
    [Description("View the health status of ASP.NET Core instance")]
    public class AspNetCoreHealthCheckWidget : StatusWidget, IPollable, IWebPage, IStandardWidget
    {
        public string URL { get; }
    }

    public class AspNetCoreHealthCheck : AsyncStatusCheck<AspNetCoreHealthCheckWidget>
    {
        protected override Task Handle(StatusRequest<AspNetCoreHealthCheckWidget> request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
