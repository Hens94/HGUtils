using HGUtils.Logging.Models;
using System.Threading.Tasks;

namespace HGUtils.Logging.Contracts
{
    public interface ILogging
    {
        Task WriteLog(RequestInfo request, ResponseInfo response);
    }
}
