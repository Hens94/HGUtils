using System;
using System.Collections;

namespace HGUtils.Logging.Models
{
    public class ResponseInfo
    {
        public DateTime ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public IDictionary Headers { get; set; }
        public string Body { get; set; }
    }
}
