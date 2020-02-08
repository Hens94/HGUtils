using HGUtils.Common.Enums;
using HGUtils.Exceptions.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using HGUtils.Common.ViewModels;
using HGUtils.Exceptions.ViewModels;

namespace HGUtils.Exceptions.Extensions
{
    public static class ExceptionExtensions
    {
        public delegate void ExecuteErrorInfo(string message, int resultCode = 999, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string detailMessage = null);
        public delegate void ExecuteErrorInfoWithException(BaseException exception);
        
        internal delegate object ConstructorDelegate(params object[] args);

        internal static T GetException<T>(
            HttpStatusCode statusCode,
            IEnumerable<ExceptionInfo> exceptionInfos) where T : BaseException
        {
            var details = JsonSerializer.Serialize(exceptionInfos);
            Log.Error($"Errores generados", details);

            var exceptionConst = CreateConstructor(typeof(T), typeof(HttpStatusCode), typeof(IEnumerable<ExceptionInfo>));
            return (T)exceptionConst(statusCode, exceptionInfos);
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
                Layer = layer,
                Service = service,
                Operation = operation,
                ExceptionName = exception.ToString(),
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

        internal static HttpContent ToErrorContent(this Exception exception)
        {
            var result = exception.ToErrorResult();
            var json = JsonSerializer.Serialize(result);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static ErrorViewModel ToErrorResult(this Exception exception)
        {
            if (exception is BaseException)
            {
                var ex = (BaseException)exception;

                return new ErrorViewModel
                {
                    Errors = ex.Errors
                };
            }

            return new ErrorViewModel
            {
                Errors = new List<ExceptionInfo>
                {
                    new ExceptionInfo
                    {
                        Code = 999,
                        UserMessage = "Ha ocurrido un error no controlado",
                        ExceptionInfoDetail = exception.GetExceptionInfo(Layer.Undefined, "Undefined", "Undefined")
                    }
                }
            };
        }
    }
}
