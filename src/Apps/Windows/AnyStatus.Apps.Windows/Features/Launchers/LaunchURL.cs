using MediatR;
using System;
using System.Diagnostics;

namespace AnyStatus.Apps.Windows.Features.Launchers
{
    public class LaunchURL
    {
        public class Request : IRequest
        {
            public Request(string url)
            {
                URL = url ?? throw new ArgumentNullException(nameof(url));
            }

            public string URL { get; }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request) => Process.Start("explorer.exe", $"\"{request.URL}\"");
        }
    }

}
