using HGUtils.Common.Enums;
using HGUtils.Exceptions.Models;
using HGUtils.Exceptions.ViewModels;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HGUtils.Exceptions.Extensions
{
    public static class ExceptionExtensions
    {
        public delegate void ExecuteErrorInfo(string message, int resultCode = 999, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string detailMessage = null);
        public delegate void ExecuteErrorInfoWithException(BaseException exception);

        internal delegate object ConstructorDelegate(params object[] args);

        internal static T GetException<T>(
            HttpStatusCode statusCode,
            IEnumerable<ExceptionInfo> exceptionInfos,
            string message = null,
            Exception ex = null) where T : BaseException
        {
            Log.Error(ex, @"
-------------------------
ERRORES GENERADOS
-------------------------
{@ExceptionInfos}
", exceptionInfos);

            var exceptionConst = CreateConstructor(typeof(T), typeof(HttpStatusCode), typeof(IEnumerable<ExceptionInfo>), typeof(string), typeof(Exception));
            return (T)exceptionConst(statusCode, exceptionInfos, message, ex);
        }

        internal static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
        {
            var constructorInfo = type.GetConstructor(parameters);

            var paramExpr = Expression.Parameter(typeof(Object[]));

            var constructorParameters = parameters.Select((paramType, index) =>
                Expression.Convert(
                    Expression.ArrayAccess(
                        paramExpr,
                        Expression.Constant(index)),
                    paramType)).ToArray();

            var body = Expression.New(constructorInfo, constructorParameters);

            var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
            return constructor.Compile();
        }

        internal static ExceptionInfoDetail GetExceptionInfo(
            this Exception exception,
            Layer layer,
            string service,
            string operation)
        {
            if (exception is null) return null;

            return new ExceptionInfoDetail
            {
                Reason = exception.Message,
                Detail = exception.StackTrace,
                Layer = layer.ToString(),
                Service = service,
                Operation = operation,
                ExceptionName = exception.GetType().Name,
                InnerError = exception.InnerException?.GetExceptionInfo(layer, service, operation)
            };
        }

        internal static void AddApiErrorHeaders(this HttpContext context)
        {
            context.Response.Headers["Content-Type"] = new[] { "application/json" };
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
        }

        internal static void AddApiErrorStatusCode(this HttpContext context, Exception exception)
        {
            context.Response.StatusCode =
                exception is BaseException ?
                (exception as BaseException).StatusCode :
                (int)HttpStatusCode.InternalServerError;
        }

        internal static HttpContent ToErrorContent(this Exception exception, bool isDevelopment = false)
        {
            var result = exception.ToErrorResult(isDevelopment);
            var json = JsonSerializer.Serialize(result);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static ErrorViewModel ToErrorResult(this Exception exception, bool isDevelopment)
        {
            if (!(exception as BaseException is null))
            {
                var ex = (BaseException)exception;

                return new ErrorViewModel
                {
                    Errors = ex.ToErrorViewModel(isDevelopment)
                };
            }

            return new ErrorViewModel
            {
                Errors = new List<ErrorItemViewModel>
                {
                    new ErrorItemViewModel
                    {
                        Code = 999,
                        Reason = "Ha ocurrido un error no controlado",
                        DevelopErrorDetail = isDevelopment ? exception.GetExceptionInfo(Layer.Undefined, "Undefined", "Undefined") : null
                    }
                }
            };
        }

        private static IEnumerable<ErrorItemViewModel> ToErrorViewModel<T>(
            this T exception, bool isDevelopment) where T : BaseException
        {
            if (exception.Errors is null || !exception.Errors.Any()) yield break;

            foreach (var error in exception.Errors)
            {
                yield return new ErrorItemViewModel
                {
                    Code = error.Code,
                    Reason = error.UserMessage,
                    DevelopErrorDetail = isDevelopment ? error.ExceptionInfoDetail : null
                };
            }
        }
    }
}
