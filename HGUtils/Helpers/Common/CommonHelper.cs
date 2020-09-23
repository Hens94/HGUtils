using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
using HGUtils.Common.Models;
using HGUtils.Common.ViewModels;
using HGUtils.Exceptions.Contracts;
using System;
using System.Net;

namespace HGUtils.Helpers.Common
{
    public static class CommonHelper
    {
        public static T UseIf<T>(this T obj, bool conditional, Func<T, T> funcIf, Func<T, T> funcElse) =>
            conditional ? funcIf(obj) : funcElse(obj);

        public static T UseIf<T>(this T obj, bool conditional, Func<T, T> funcIf) =>
            conditional ? funcIf(obj) : obj;

        public static bool IsSuccess(this IResult result) =>
            result is null ?
            false :
            Enum.TryParse<ResultType>(result.Code.ToString(), out var resultType) && resultType.Equals(ResultType.Success);

        public static IResult ToResultWithPagination<TResult>(
            this TResult result,
            IPaginationResponse pagination) where TResult : class =>
            new ResultWithPagination<TResult>(result, pagination);

        public static IResult ToResult<TResult>(
            this TResult result) where TResult : class =>
            new Result<TResult>(result);

        public static (TResult, IPaginationResponse) ValidateAndGetDataFromResult<TResult>(
            this IResult result,
            bool usePagination,
            IExceptionHandler.ExecError execError) where TResult : class
        {
            if (!result.IsSuccess())
                execError(HttpStatusCode.InternalServerError);

            if (!usePagination) return (((IResult<TResult>)result).Results, null);

            var resultWithPagination = (ResultWithPagination<TResult>)result;

            return (resultWithPagination.Results, resultWithPagination.Pagination);
        }

        public static IResult ToSuccessResultViewModel() =>
            new ResultViewModel
            {
                Code = 0,
                Message = "El servicio se ha consumido correctamente",
                DetailMessage = "El servicio se ha consumido correctamente"
            };

        public static IResult ToSuccessResultViewModel<TResult>(this TResult result) where TResult : class =>
            new ResultViewModel<TResult>
            {
                Code = 0,
                Message = "El servicio se ha consumido correctamente",
                DetailMessage = "El servicio se ha consumido correctamente",
                Results = result
            };

        public static IResult ToSuccessResultViewModel<TResult>(this TResult result, IPaginationResponse pagination) where TResult : class =>
            pagination is null ?
            new ResultViewModel<TResult>
            {
                Code = 0,
                Message = "El servicio se ha consumido correctamente",
                DetailMessage = "El servicio se ha consumido correctamente",
                Results = result
            } :
            new ResultWithPaginationViewModel<TResult>
            {
                Code = 0,
                Message = "El servicio se ha consumido correctamente",
                DetailMessage = "El servicio se ha consumido correctamente",
                Results = result,
                Pagination = pagination
            };
    }
}
