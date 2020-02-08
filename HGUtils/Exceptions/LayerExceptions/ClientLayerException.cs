using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace HGUtils.Exceptions.LayerExceptions
{
    [Serializable]
    public class ClientLayerException : BaseException
    {
        public ClientLayerException(
            HttpStatusCode statusCode,
            IEnumerable<ExceptionInfo> errors,
            string message = "Ha ocurrido un error no controlado (ClientLayerException)",
            Exception ex = null) : base(statusCode, errors, message, ex)
        {
        }

        protected ClientLayerException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
