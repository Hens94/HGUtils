using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Common.Models
{
    public class ErrorItem
    {
        public int Code { get; set; }
        public string UserMessage { get; set; }
        public DevelopInfo DevelopInfo { get; set; }
    }
}
