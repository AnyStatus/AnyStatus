using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Core.App;
using MediatR;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Features
{
    public sealed class Save
    {
        public class Request : IRequest<bool>
        {
            public Request(bool showDialog = false) => ShowDialog = showDialog;

            public bool ShowDialog { get; set; }
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly IAppContext _context;
            private readonly IDialogService _dialogService;

            public Handler(IAppContext context, IDialogService dialogService)
            {
                _context = context;
                _dialogService = dialogService;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.ShowDialog || string.IsNullOrEmpty(_context.Session.FileName))
                {
                    var dialog = new SaveFileDialog("JSON|*.json");

                    if (_dialogService.ShowFileDialog(dialog) == DialogResult.OK)
                    {
                        _context.Session.FileName = dialog.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }

                await Save(_context.Session.Widget, _context.Session.FileName, cancellationToken);

                _context.Session.IsDirty = false;

                return true;
            }

            private async Task Save(IWidget widget, string fileName, CancellationToken cancellationToken)
            {
                var directory = Path.GetDirectoryName(fileName);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(widget, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                var bytes = new UTF8Encoding().GetBytes(json);

                using var stream = File.Open(fileName, FileMode.Create);

                stream.Seek(0, SeekOrigin.End);

                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}