using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Exceptions
{
    public interface IException
    {
        public int StatusCode { get; set; }
        public int ResultCode { get; set; }
        public string DetailMessage { get; set; }
    }
}
