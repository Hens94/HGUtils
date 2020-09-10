using HGUtils.Logging.Contracts;
using HGUtils.Logging.Extensions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace HGUtils.Logging.Middleware
{
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ILogging _logger;

        public LoggingMiddleware(ILogging logger)
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
