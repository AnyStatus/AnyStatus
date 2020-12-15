using AnyStatus.API.Widgets;
using MediatR;
using System.IO;

namespace AnyStatus.Plugins.SystemInformation.FileSystem
{
    public class FileExistsCheck : RequestHandler<StatusRequest<FileExistsWidget>>
    {
        protected override void Handle(StatusRequest<FileExistsWidget> request) => request.Context.Status = File.Exists(request.Context.Path) ? Status.OK : Status.Failed;
    }
}
