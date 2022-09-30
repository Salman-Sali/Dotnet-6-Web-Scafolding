using System.Net;

namespace Shared.Exceptions
{
    public class AppException : ApplicationException
    {
        public AppException(string systemMessage, string friendlyMessage, HttpStatusCode httpStatusCode) : base(systemMessage)
        {
            FriendlyMessage = friendlyMessage;
            HttpStatusCode = httpStatusCode;
            SystemMessage = systemMessage;
        }

        public AppException(string friendlyMessage, HttpStatusCode httpStatusCode) : base(friendlyMessage)
        {
            FriendlyMessage = friendlyMessage;
            HttpStatusCode = httpStatusCode;
        }

        public string SystemMessage { get; }
        public string FriendlyMessage { get; }
        public HttpStatusCode HttpStatusCode { get; }
    }
}
