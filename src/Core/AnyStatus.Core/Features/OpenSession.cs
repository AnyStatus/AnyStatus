using AnyStatus.API.Dialogs;
using AnyStatus.API.Widgets;
using AnyStatus.Core.App;
using AnyStatus.Core.Jobs;
using AnyStatus.Core.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Core.Features
{
    public class OpenSession
    {
        public class Request : IRequest<bool>
        {
            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly ILogger _logger;
            private readonly IAppContext _context;
            private readonly IJobScheduler _jobScheduler;
            private readonly IDialogService _dialogService;
            private readonly ContractResolver _resolver;

            public Handler(IJobScheduler jobScheduler, IDialogService dialogService, IAppContext context, ILogger logger, ContractResolver resolver)
            {
                _logger = logger;
                _context = context;
                _resolver = resolver;
                _jobScheduler = jobScheduler;
                _dialogService = dialogService;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                if (_context.Session is null)
                {
                    throw new Exception("Session not found");
                }

                if (string.IsNullOrEmpty(request.FileName))
                {
                    var dialog = new OpenFileDialog("JSON|*.json");

                    var result = _dialogService.ShowFileDialog(dialog);

                    if (result != DialogResult.OK)
                    {
                        return false;
                    }

                    request.FileName = dialog.FileName;
                }

                if (string.IsNullOrEmpty(request.FileName) || !File.Exists(request.FileName))
                {
                    throw new FileNotFoundException(request.FileName);
                }

                _context.Session.IsDirty = false;

                _context.Session.FileName = request.FileName;

                _context.Session.Widget = GetWidget(request.FileName);

                await _jobScheduler.ClearAsync(cancellationToken);

                await Schedule(_context.Session.Widget, cancellationToken);

                return true;
            }

            private async Task Schedule(IWidget widget, CancellationToken cancellationToken)
            {
                if (widget is IPollable)
                {
                    await _jobScheduler.ScheduleJobAsync(widget.Id, widget, cancellationToken);

                    _logger.LogDebug("Widget '{widget}' job is running...", widget.Name);
                }

                if (widget.HasChildren)
                {
                    foreach (var child in widget)
                    {
                        await Schedule(child, cancellationToken);
                    }
                }
            }

            private IWidget GetWidget(string fileName)
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException(fileName);
                }

                var json = File.ReadAllText(fileName);

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new FileLoadException("File is empty.");
                }

                return JsonConvert.DeserializeObject<IWidget>(json, new JsonSerializerSettings
                {
                    ContractResolver = _resolver,
                    TypeNameHandling = TypeNameHandling.All,
                    Converters = new[] { new WidgetConverter(_logger) }
                });
            }
        }
    }
}