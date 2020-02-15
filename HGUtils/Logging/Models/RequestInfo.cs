using System;
using System.Collections;

namespace HGUtils.Logging.Models
{
    public class RequestInfo
    {
        public DateTime RequestTime { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public IDictionary Headers { get; set; }
        public string Body { get; set; }
    }
}
