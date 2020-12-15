using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Demo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AnyStatus.Core.Tests.Integration
{
    public class PipelineTests : IClassFixture<ContainerFixture>
    {
        private readonly IMediator _mediator;

        public PipelineTests(ContainerFixture fixture) => _mediator = fixture.Container.GetInstance<IMediator>();

        [Fact]
        public async Task HealthCheckTest()
        {
            var widget = new DemoHealthCheckWidget
            {
                Randomize = false,
                Name = "Health check test"
            };

            var request = StatusRequestFactory.Create(widget);

            await _mediator.Send(request, CancellationToken.None);

            Assert.Same(Status.OK, widget.Status);
        }

        [Fact]
        public async Task MetricTest()
        {
            var widget = new DemoMetricWidget
            {
                Randomize = false
            };

            var request = MetricRequestFactory.Create(widget);

            await _mediator.Send(request, CancellationToken.None);

            Assert.Equal(1, widget.Value);

            Assert.Equal(Status.OK, widget.Status);
        }
    }
}
