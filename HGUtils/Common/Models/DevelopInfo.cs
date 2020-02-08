using System;

namespace HGUtils.Common.Models
{
    public class DevelopInfo
    {
        public string Reason { get; set; }
        public string Detail { get; set; }
        public DateTime Time { get; set; }
        public string Layer { get; set; }
        public string Service { get; set; }
        public string Operation { get; set; }
        public string ExceptionName { get; set; }
        public DevelopInfo InnerError { get; set; }
    }
}
