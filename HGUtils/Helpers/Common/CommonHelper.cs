using System;

namespace HGUtils.Helpers.Common
{
    public static class CommonHelper
    {
        public static T UseIf<T>(this T obj, bool conditional, Func<T, T> funcIf, Func<T, T> funcElse) =>
            conditional ? funcIf(obj) : funcElse(obj);

        public static T UseIf<T>(this T obj, bool conditional, Func<T, T> funcIf) =>
            conditional ? funcIf(obj) : obj;
    }
}
