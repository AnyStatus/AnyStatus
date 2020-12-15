using AnyStatus.Core.Settings;
using AnyStatus.Plugins.Demo;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AnyStatus.Core.Tests.Integration
{
    public sealed class PersistenceTests : IClassFixture<ContainerFixture>, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly string _tempFileName;

        public PersistenceTests(ContainerFixture fixture)
        {
            _mediator = fixture.Container.GetInstance<IMediator>();

            _tempFileName = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(_tempFileName))
            {
                File.Delete(_tempFileName);
            }
        }

        [Fact]
        public async Task SaveAndLoadWidgetTest()
        {
            var widget = new DemoHealthCheckWidget
            {
                Name = "Health Check"
            };

            var writeRequest = new SaveWidget.Request
            {
                FileName = _tempFileName,
                Widget = widget
            };

            var writeResponse = await _mediator.Send(writeRequest, CancellationToken.None);

            Assert.True(writeResponse);

            Assert.True(File.Exists(_tempFileName));

            var readResponse = await _mediator.Send(new GetWidget.Request(_tempFileName), CancellationToken.None);

            Assert.NotNull(readResponse);
            Assert.Equal(widget.Id, readResponse.Id);
            Assert.Equal(widget.Name, readResponse.Name);
        }
    }
}
