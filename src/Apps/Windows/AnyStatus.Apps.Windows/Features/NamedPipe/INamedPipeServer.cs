using System.Threading.Tasks;

namespace AnyStatus.Apps.Windows.Features.NamedPipe
{
    internal interface INamedPipeServer
    {

        Task StartAsync();
    }
}
