using HGUtils.Common.Enums;
using HGUtils.Exceptions.Contracts;
using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private void AddError<TException>(
            string userMessage,
            ResultType resultCode = ResultType.ApiError,
            string reason = null,
            string detail = null,
            Layer layer = Layer.Undefined,
            string service = null,
            string operation = null) where TException : BaseException
        {
            exceptions.Add(new ExceptionInfo
            {
                Code = (int)resultCode,
                UserMessage = userMessage,
                ExceptionInfoDetail = new ExceptionInfoDetail
                {
                    Reason = reason ?? "Undefined",
                    Detail = detail ?? reason ?? "Undefined",
                    Layer = layer.ToString(),
                    Service = service,
                    Operation = operation,
                    ExceptionName = typeof(TException).Name
                }
            });
        }

        private void ProcessCatch<TException>(
            Exception ex,
            string genericErrorMessage,
            Layer layer,
            string service,
            string operation,
            bool throwGenericException) where TException : BaseException
        {
            exceptions.Add(new ExceptionInfo
            {
                Code = 999,
                UserMessage = genericErrorMessage ?? "Ha ocurrido un error no controlado",
                ExceptionInfoDetail = ex.GetExceptionInfo(layer, service, operation)
            });

            if (throwGenericException)
            {
                throw GetException<TException>(HttpStatusCode.InternalServerError, exceptions);
            }
        }

        private void CheckExceptionInfo<TException>(
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null) where TException : BaseException
        {
            if (!exceptions.Any())
            {
                exceptions.Add(new ExceptionInfo
                {
                    Code = 999,
                    UserMessage = genericErrorMessage ?? "Ha ocurrido un error no controlado",
                    ExceptionInfoDetail = new ExceptionInfoDetail
                    {
                        Reason = "Undefined",
                        Detail = "Undefined",
                        Layer = layer.ToString(),
                        Service = service,
                        Operation = operation,
                        ExceptionName = typeof(TException).Name
                    }
                });
            }
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
                    (userMessage, resultCode, reason, detail) => AddError<TException>(userMessage, resultCode, reason, detail, layer, service, operation),
                    (statusCode) =>
                    {
                        CheckExceptionInfo<TException>(layer, service, operation, genericErrorMessage);
                        throw GetException<TException>(statusCode, exceptions);
                    });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ProcessCatch<TException>(ex, genericErrorMessage, layer, service, operation, throwGenericException);
                return default;
            }
        }

        public async Task UseCatchExceptionAsync<TException>(
            Func<IExceptionHandler.AddErrorInfo, IExceptionHandler.ExecError, Task> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException
        {
            try
            {
                await func(
                    (userMessage, resultCode, reason, detail) => AddError<TException>(userMessage, resultCode, reason, detail, layer, service, operation),
                    (statusCode) =>
                    {
                        CheckExceptionInfo<TException>(layer, service, operation, genericErrorMessage);
                        throw GetException<TException>(statusCode, exceptions);
                    });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ProcessCatch<TException>(ex, genericErrorMessage, layer, service, operation, throwGenericException);
            }
        }

        public T UseCatchException<T, TException>(
            Func<IExceptionHandler.AddErrorInfo, IExceptionHandler.ExecError, T> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException
        {
            try
            {
                return func(
                    (userMessage, resultCode, reason, detail) => AddError<TException>(userMessage, resultCode, reason, detail, layer, service, operation),
                    (statusCode) =>
                    {
                        CheckExceptionInfo<TException>(layer, service, operation, genericErrorMessage);
                        throw GetException<TException>(statusCode, exceptions);
                    });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ProcessCatch<TException>(ex, genericErrorMessage, layer, service, operation, throwGenericException);
                return default;
            }
        }

        public void UseCatchException<TException>(
            Action<IExceptionHandler.AddErrorInfo, IExceptionHandler.ExecError> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException
        {
            try
            {
                func(
                    (userMessage, resultCode, reason, detail) => AddError<TException>(userMessage, resultCode, reason, detail, layer, service, operation),
                    (statusCode) =>
                    {
                        CheckExceptionInfo<TException>(layer, service, operation, genericErrorMessage);
                        throw GetException<TException>(statusCode, exceptions);
                    });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ProcessCatch<TException>(ex, genericErrorMessage, layer, service, operation, throwGenericException);
            }
        }
    }
}
