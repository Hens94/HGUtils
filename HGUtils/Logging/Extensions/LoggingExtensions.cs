using HGUtils.Logging.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HGUtils.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static async Task<RequestInfo> FormatRequest(this HttpRequest request)
        {
            request.EnableBuffering();

            var resquestBody = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = 0;

            return new RequestInfo
            {
                RequestTime = DateTime.Now,
                Uri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Method = request.Method,
                Body = string.IsNullOrEmpty(resquestBody) ? null : resquestBody
            };
        }

        public static async Task<ResponseInfo> FormatResponse(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return new ResponseInfo
            {
                ResponseTime = DateTime.Now,
                StatusCode = response.StatusCode,
                Body = string.IsNullOrEmpty(responseBody) ? null : responseBody
            };
        }
    }
}
