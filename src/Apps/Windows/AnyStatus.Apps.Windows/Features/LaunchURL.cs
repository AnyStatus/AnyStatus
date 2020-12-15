using MediatR;
using System;
using System.Diagnostics;

namespace AnyStatus.Apps.Windows.Features
{
    public class LaunchURL
    {
        public class Request : IRequest
        {
            public Request(string url)
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url));
                }

                URL = url;
            }

            public string URL { get; }
        }

        public class Handler : RequestHandler<Request>
        {
            protected override void Handle(Request request)
            {
                Process.Start("explorer.exe", $"\"{request.URL}\"");
            }
        }
    }

}
