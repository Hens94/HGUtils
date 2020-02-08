using HGUtils.Exceptions.Models;
using System.Collections.Generic;

namespace HGUtils.Exceptions
{
    public interface IException
    {
        int StatusCode { get; set; }
        IEnumerable<ExceptionInfo> Errors { get; set; }
    }
}
