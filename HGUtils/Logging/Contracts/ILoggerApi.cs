using HGUtils.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Logging.Contracts
{
    public interface ILoggerApi
    {
        Task WriteLog(RequestInfo request, ResponseInfo response);
    }
}
