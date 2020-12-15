using AnyStatus.API.Widgets;
using MediatR;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Settings
{
    public class SaveWidget
    {
        public class Request : IRequest<bool>
        {
            public string FileName { get; set; }

            public IWidget Widget { get; set; }
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var directory = Path.GetDirectoryName(request.FileName);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(request.Widget, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                var bytes = new UTF8Encoding().GetBytes(json);

                using (var stream = File.Open(request.FileName, FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.End);

                    await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
                }

                return true;
            }
        }
    }
}
