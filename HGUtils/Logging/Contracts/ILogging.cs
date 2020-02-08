using HGUtils.Logging.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Logging.Contracts
{
    public interface ILogging
    {
        Task WriteLog(RequestInfo request, ResponseInfo response);
    }
}
