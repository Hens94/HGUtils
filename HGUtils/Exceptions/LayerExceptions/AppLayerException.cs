using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace HGUtils.Exceptions.LayerExceptions
{
    [Serializable]
    public class AppLayerException : BaseException
    {
        public AppLayerException(
            HttpStatusCode statusCode,
            IEnumerable<ExceptionInfo> errors,
            string message = "Ha ocurrido un error no controlado (AppLayerException)",
            Exception ex = null) : base(statusCode, errors, message, ex)
        {
        }

        protected AppLayerException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
