using HGUtils.Logging.Contracts;
using HGUtils.Logging.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Logging.Middleware
{
    public class LoggerMiddleware : IMiddleware
    {
        private readonly ILoggerApi _logger;

        public LoggerMiddleware(ILoggerApi logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = await context.Request.FormatRequest();

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;

            await next(context);

            var response = await context.Response.FormatResponse();

            await _logger.WriteLog(request, response);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
