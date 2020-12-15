using System;
using System.Runtime.Serialization;

//demo server https://www.xml2selenium.com/demo-build-at-jenkins/

namespace AnyStatus.Plugins.Jenkins.API
{
    [Serializable]
    internal class JenkinsApiException : Exception
    {
        public JenkinsApiException()
        {
        }

        public JenkinsApiException(string message) : base(message)
        {
        }

        public JenkinsApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JenkinsApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
