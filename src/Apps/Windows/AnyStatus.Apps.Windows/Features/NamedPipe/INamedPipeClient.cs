namespace AnyStatus.Apps.Windows.Features.NamedPipe
{
    public interface INamedPipeClient
    {
        void Send(string message);
    }
}