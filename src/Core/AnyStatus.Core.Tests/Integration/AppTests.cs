using AnyStatus.API.Widgets;
using AnyStatus.Core.Jobs;
using AnyStatus.Plugins;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AnyStatus.Plugins.Demo;
using Xunit;
using Xunit.Priority;

namespace AnyStatus.Core.Tests.Integration
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AppTests : IClassFixture<ContainerFixture>
    {
        private readonly IMediator _mediator;

        public AppTests(ContainerFixture fixture)
        {
            _mediator = fixture.Container.GetInstance<IMediator>();
        }

        [Fact, Priority(0)]
        public async Task StartAppTest()
        {
            var request = new StartScheduler.Request();

            await _mediator.Send(request, CancellationToken.None);
        }

        [Fact, Priority(1)]
        public async Task ScheduleHealthCheckJobTest()
        {
            var widget = new DemoHealthCheckWidget
            {
                Randomize = false,
                Status = Status.None,
                Name = "Health Check Test",
            };

            var request = new ScheduleJob.Request(widget);

            await _mediator.Send(request, CancellationToken.None);

            await Task.Delay(1000);

            Assert.Equal(Status.OK, widget.Status);
        }

        [Fact, Priority(2)]
        public async Task ScheduleMetricJobTest()
        {
            var widget = new DemoMetricWidget
            {
                Randomize = false,
                Status = Status.None,
                Name = "Metric Test",
            };

            var request = new ScheduleJob.Request(widget);

            await _mediator.Send(request, CancellationToken.None);

            await Task.Delay(1000);

            Assert.Equal(Status.OK, widget.Status);
        }

        [Fact, Priority(3)]
        public async Task StopAppTest()
        {
            var request = new StopScheduler.Request();

            await _mediator.Send(request, CancellationToken.None);
        }
    }
}
