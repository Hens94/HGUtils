using System;

namespace HGUtils.Exceptions.Models
{
    public class ExceptionInfoDetail
    {
        public string ErrorType { get; set; } = "Undefined";
        public string Reason { get; set; } = "Undefined";
        public string Detail { get; set; } = "Undefined";
        public DateTime Time => DateTime.Now;
        public string Layer { get; set; } = "Undefined";
        public string Service { get; set; } = "Undefined";
        public string Operation { get; set; } = "Undefined";
        public string ExceptionName { get; set; } = "Undefined";
        public ExceptionInfoDetail InnerError { get; set; }
    }
}
