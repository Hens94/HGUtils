using System;
using System.Collections.Generic;

namespace HGUtils.Logging.Models
{
    public class ResponseInfo
    {
        public DateTime ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
