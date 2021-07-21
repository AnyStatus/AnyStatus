using AnyStatus.Core.App;
using MediatR;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Settings
{
    public class SaveUserSettings
    {
        public class Request : IRequest<bool>
        {
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IAppContext _context;
            private readonly IAppSettings _appSettings;

            public Handler(IAppSettings appSettings, IAppContext context)
            {
                _context = context;
                _appSettings = appSettings;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var directory = Path.GetDirectoryName(_appSettings.UserSettingsFilePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(_context.UserSettings, Formatting.Indented);

                var bytes = new UTF8Encoding().GetBytes(json);

                using (var stream = File.Open(_appSettings.UserSettingsFilePath, FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.End);

                    await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
                }

                return true;
            }
        }
    }
}
