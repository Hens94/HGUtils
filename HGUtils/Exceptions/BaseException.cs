using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace HGUtils.Exceptions
{
    [Serializable]
    public abstract class BaseException : Exception, IException
    {
        public int StatusCode { get; set; }
        public int ResultCode { get; set; }
        public string DetailMessage { get; set; }

        protected BaseException(
            string message,
            int resultCode,
            HttpStatusCode statusCode,
            string detailMessage,
            string exceptionType) : base(message)
        {
            ResultCode = resultCode;
            StatusCode = (int)statusCode;
            DetailMessage = $"({exceptionType}) {detailMessage ?? message}";
        }

        protected BaseException(
            Exception exception,
            string message,
            int resultCode,
            HttpStatusCode statusCode,
            string detailMessage,
            string exceptionType) : base(message, exception)
        {
            ResultCode = resultCode;
            StatusCode = (int)statusCode;
            DetailMessage = $"({exceptionType}) {detailMessage ?? exception?.InnerException?.Message ?? exception?.Message ?? message}";
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
