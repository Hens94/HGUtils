using System;
using System.Collections.Generic;

namespace HGUtils.Logging.Models
{
    public class RequestInfo
    {
        public DateTime RequestTime { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
