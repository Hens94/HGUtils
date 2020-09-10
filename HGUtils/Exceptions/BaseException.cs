using HGUtils.Exceptions;
using HGUtils.Exceptions.Models;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace System
{
    [Serializable]
    public abstract class BaseException : Exception, IException
    {
        public int StatusCode { get; set; }
        public IEnumerable<ExceptionInfo> Errors { get; set; }

        protected BaseException(
            HttpStatusCode statusCode,
            IEnumerable<ExceptionInfo> errors,
            string message = "Ha ocurrido un error no controlado",
            Exception ex = null) : base(message, ex)
        {
            StatusCode = (int)statusCode;
            Errors = errors;
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
