using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Logging.Models
{
    public class ResponseInfo
    {
        public DateTime ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public string Body { get; set; }
    }
}
