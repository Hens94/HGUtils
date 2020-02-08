using HGUtils.Exceptions.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Exceptions.Middleware
{
    internal class ErrorMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.AddApiErrorHeaders();
                context.AddApiErrorStatusCode(ex);
                await context.Response.WriteAsync(await ex.ToErrorContent().ReadAsStringAsync(), Encoding.UTF8);
            }
        }
    }
}
