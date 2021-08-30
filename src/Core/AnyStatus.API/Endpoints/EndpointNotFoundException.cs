using System;

namespace AnyStatus.API.Endpoints
{
    public class EndpointNotFoundException : Exception
    {
        public EndpointNotFoundException()
        {
        }

        public EndpointNotFoundException(string message) : base(message)
        {
        }
    }
}
