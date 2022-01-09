using AnyStatus.API.Services;
using AnyStatus.API.Widgets;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zeroconf;

namespace AnyStatus.Plugins.SystemInformation.Network.Zeroconf
{
    public class ZeroconfScanner : AsyncStatusCheck<ZeroconfDevicesWidget>
    {
        private readonly IDispatcher _dispatcher;

        public ZeroconfScanner(IDispatcher dispatcher) => _dispatcher = dispatcher;

        protected override async Task Handle(StatusRequest<ZeroconfDevicesWidget> request, CancellationToken cancellationToken)
        {
            var domains = await ZeroconfResolver.BrowseDomainsAsync().ConfigureAwait(false);

            var hosts = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));

            _dispatcher.Invoke(() => 
                new ZeroconfSynchronizer(request.Context)
                        .Synchronize(hosts.ToList(), request.Context.OfType<ZeroconfDeviceWidget>().ToList()));

            request.Context.Status = Status.OK;
        }
    }
}
