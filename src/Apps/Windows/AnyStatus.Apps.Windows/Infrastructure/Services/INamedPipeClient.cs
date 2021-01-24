namespace AnyStatus.Apps.Windows
{
    public interface INamedPipeClient
    {
        void Send(string message);
    }
}