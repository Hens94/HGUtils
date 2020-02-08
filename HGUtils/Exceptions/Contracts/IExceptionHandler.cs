using HGUtils.Common.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HGUtils.Exceptions.Contracts
{
    public interface IExceptionHandler
    {
        delegate void AddErrorInfo(
            string userMessage,
            int resultCode = 999,
            string reason = null,
            string detail = null);

        delegate void ExecError(HttpStatusCode statusCode = HttpStatusCode.InternalServerError);

        Task<T> UseCatchExceptionAsync<T, TException>(
            Func<AddErrorInfo, ExecError, Task<T>> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException;
    }
}
