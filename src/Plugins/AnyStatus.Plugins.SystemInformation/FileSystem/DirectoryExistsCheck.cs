using AnyStatus.API.Widgets;
using MediatR;
using System.IO;

namespace AnyStatus.Plugins.SystemInformation.FileSystem
{
    public class DirectoryExistsCheck : RequestHandler<StatusRequest<DirectoryExistsWidget>>
    {
        protected override void Handle(StatusRequest<DirectoryExistsWidget> request) => request.Context.Status = Directory.Exists(request.Context.Path) ? Status.OK : Status.Failed;
    }
}
