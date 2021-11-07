using AnyStatus.API.Widgets;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Demo
{
    [Category("Demo")]
    [DisplayName("Health Check Demo")]
    [Description("A demo health check widget with random status")]
    public class DemoHealthCheckWidget : StatusWidget, IPollable, IWebPage, IStandardWidget
    {
        public bool Randomize { get; set; } = true;

        public string URL => "https://www.anystat.us";
    }

    public class TestHealthChecker : AsyncStatusCheck<DemoHealthCheckWidget>
    {
        private readonly ILogger _logger;

        public TestHealthChecker(ILogger logger) => _logger = logger;

        protected override Task Handle(StatusRequest<DemoHealthCheckWidget> request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Testing {request.Context.Name}");

            Status status;

            if (request.Context.Randomize)
            {
                var rnd = new Random();

                Status.TryParse(rnd.Next(0, 10), out status);
            }
            else
            {
                status = Status.OK;
            }

            request.Context.Status = status;

            return Unit.Task;
        }
    }
}
