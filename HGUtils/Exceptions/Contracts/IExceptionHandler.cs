﻿using HGUtils.Common.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HGUtils.Exceptions.Contracts
{
    public interface IExceptionHandler
    {
        delegate void AddErrorInfo(
            string userMessage,
            ResultType resultCode = ResultType.ApiError,
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

        Task UseCatchExceptionAsync<TException>(
            Func<AddErrorInfo, ExecError, Task> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException;

        T UseCatchException<T, TException>(
            Func<AddErrorInfo, ExecError, T> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException;

        void UseCatchException<TException>(
            Action<AddErrorInfo, ExecError> func,
            Layer layer,
            string service,
            string operation,
            string genericErrorMessage = null,
            bool throwGenericException = true) where TException : BaseException;
    }
}
