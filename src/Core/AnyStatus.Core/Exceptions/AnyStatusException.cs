using System;

namespace AnyStatus.Core
{
    public class AnyStatusException : Exception
    {
        public AnyStatusException(string message) : base(message)
        {
        }

        public AnyStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AnyStatusException(string message, Exception innerException, bool transient) : base(message, innerException)
        {
            Transient = transient;
        }

        public bool Transient { get; }
    }
}
