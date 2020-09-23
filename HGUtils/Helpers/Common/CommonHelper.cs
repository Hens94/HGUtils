using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
using HGUtils.Common.Models;
using System;

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

        public static IResult ToResultWithPaginationData<TResult>(
            this TResult result,
            IPaginationResponse pagination) where TResult : class =>
            new ResultWithPagination<TResult>(result, pagination);

        public static IResult ToResultData<TResult>(
            this TResult result) where TResult : class =>
            new Result<TResult>(result);
    }
}
