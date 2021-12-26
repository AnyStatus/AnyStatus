using Microsoft.Extensions.Logging;
using System;

namespace AnyStatus.Core.Logging
{
    public class ActivityMessage
    {
        public DateTime Time { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public int ThreadId { get; set; }

        public Exception Exception { get; set; }

        public bool HasException => Exception is not null;
    }
}
