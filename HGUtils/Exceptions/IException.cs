using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Exceptions
{
    public interface IException
    {
        int StatusCode { get; set; }
        IEnumerable<ExceptionInfo> Errors { get; set; }
    }
}
