using HGUtils.Common.Enums;
using HGUtils.Exceptions.Contracts;
using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static HGUtils.Exceptions.Extensions.ExceptionExtensions;

namespace HGUtils.Exceptions.Services
{
    public class ExceptionHandlerService : IExceptionHandler
    {
        private readonly ICollection<ExceptionInfo> exceptions;

        public ExceptionHandlerService()
        {
            exceptions = new List<ExceptionInfo>();
        }


        public async Task<T> UseCatchExceptionAsync<T, TException>(
            Func<IExceptionHandler.AddErrorInfo, IExceptionHandler.ExecError, Task<T>> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException
        {
            try
            {
                return await func(
                    (userMessage, resultCode, reason, detail) =>
                    {
                        exceptions.Add(new ExceptionInfo
                        {
                            Code = resultCode,
                            UserMessage = userMessage,
                            ExceptionInfoDetail = new ExceptionInfoDetail
                            {
                                Reason = reason ?? "Undefined",
                                Detail = detail ?? reason ?? "Undefined",
                                Layer = layer,
                                Service = service,
                                Operation = operation,
                                ExceptionName = typeof(TException).Name
                            }
                        });
                    },
                    (statusCode) =>
                    {
                        throw GetException<TException>(statusCode, exceptions);
                    });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (throwGenericException)
                {
                    throw GetException<TException>(message: genericErrorMessage, exception: ex);
                }
                else
                {
                    exceptions.Add(new ExceptionInfo
                    {
                        Code = 999,
                        UserMessage = genericErrorMessage ?? "Ha ocurrido un error no controlado",
                        ExceptionInfoDetail = ex.GetExceptionInfo(layer, service, operation)
                    });
                }                
            }
        }
    }
}
