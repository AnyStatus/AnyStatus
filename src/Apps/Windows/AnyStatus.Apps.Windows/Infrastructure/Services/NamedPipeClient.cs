using System.IO;
using System.IO.Pipes;

namespace AnyStatus.Apps.Windows
{
    internal class NamedPipeClient : INamedPipeClient
    {
        private const string _pipeName = "{89790288-AE14-4BE1-A2D2-501EBC3F9C9E}"; //move to app.config

        public void Send(string message)
        {
            using var client = new NamedPipeClientStream(_pipeName);

            client.Connect(1000);

            using var writer = new StreamWriter(client);

            writer.WriteLine(message);

            writer.Flush();
        }
    }
}
