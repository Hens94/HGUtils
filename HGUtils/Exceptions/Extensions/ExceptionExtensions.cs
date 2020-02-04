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

namespace HGUtils.Exceptions.Extensions
{
    public static class ExceptionExtensions
    {
        public delegate void ExecuteErrorInfo(string message, int resultCode = 999, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string detailMessage = null);
        public delegate void ExecuteErrorInfoWithException(BaseException exception);
        
        internal delegate object ConstructorDelegate(params object[] args);

        internal static T GetException<T>(HttpStatusCode statusCode, IEnumerable<ExceptionInfo> exceptionInfos) where T : BaseException
        {
            if (exception is null)
            {
                var exceptionConst = CreateConstructor(typeof(T), typeof(string), typeof(int), typeof(HttpStatusCode), typeof(string));
                return (T)exceptionConst(message, resultCode, statusCode, detailMessage);
            }
            else
            {
                Log.Error(exception, $"Ha ocurrido un error no controlado: {exception.Source}");

                var exceptionConst = CreateConstructor(typeof(T), typeof(Exception), typeof(string), typeof(int), typeof(HttpStatusCode), typeof(string));
                return (T)exceptionConst(exception, message, resultCode, statusCode, detailMessage);
            }
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
    }
}
