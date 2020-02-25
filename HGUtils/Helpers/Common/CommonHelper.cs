using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
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
            Enum.TryParse<ResultType>(result.Code.ToString(), out var resultType) ? resultType.Equals(ResultType.Success) : false;
    }
}
