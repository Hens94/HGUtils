using HGUtils.Helpers.HttpClient;
using HGUtils.Logging.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HGUtils.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static async Task<RequestInfo> FormatRequest(this HttpRequest request)
        {
            request.EnableBuffering();

            string requestBody;

            if (request.HasFormContentType && (request.Form?.Files?.Any() ?? false))
            {
                var formModel = request.Form.Files.Select(x => new { x.FileName, x.Name, x.ContentType, x.ContentDisposition, x.Length });
                requestBody = JsonSerializer.Serialize(formModel);
            }
            else
            {
                requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            }

            request.Body.Position = 0;

            return new RequestInfo
            {
                RequestTime = DateTime.Now,
                Uri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Method = request.Method,
                Headers = request.Headers?.ToDictionary(x => x.Key, x => string.Join(";", x.Value)),
                Body = string.IsNullOrEmpty(requestBody) ? null : requestBody
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
                Headers = response.Headers?.ToDictionary(x => x.Key, x => string.Join(";", x.Value)),
                Body = string.IsNullOrEmpty(responseBody) ? null : responseBody
            };
        }

        public static async Task<RequestInfo> FormatRequest(this HttpRequestMessage request)
        {
            var requestClone = request.Clone();
            var requestBody = requestClone.Content is null ? null : await requestClone.Content.ReadAsStringAsync();

            return new RequestInfo
            {
                RequestTime = DateTime.Now,
                Uri = request.RequestUri.ToString(),
                Method = request.Method.ToString(),
                Headers = request.Headers?.ToDictionary(x => x.Key, x => string.Join(";", x.Value)),
                Body = string.IsNullOrEmpty(requestBody) ? null : requestBody
            };
        }

        public static async Task<ResponseInfo> FormatResponse(this HttpResponseMessage response)
        {
            var responseClone = response.Clone();
            var responseBody = responseClone.Content is null ? null : await responseClone.Content.ReadAsStringAsync();

            return new ResponseInfo
            {
                ResponseTime = DateTime.Now,
                StatusCode = (int)response.StatusCode,
                Headers = response.Headers?.ToDictionary(x => x.Key, x => string.Join(";", x.Value)),
                Body = string.IsNullOrEmpty(responseBody) ? null : responseBody
            };
        }

        public static void WriteApiRequestInLog(this RequestInfo request, LogEventLevel eventLevel = LogEventLevel.Information)
        {
            Log.Write(eventLevel, @"
-------------------------
THIRD PARTY REQUEST
-------------------------
URI:
{RequestUri}

METHOD:
{Method}

HEADERS:
{Headers}

BODY:
{@Body}
", request.Uri, request.Method, request.Headers, request.Body);
        }

        public static void WriteApiResponseInLog(this ResponseInfo response, LogEventLevel eventLevel = LogEventLevel.Information)
        {
            Log.Write(eventLevel, @"
-------------------------
THIRD PARTY RESPONSE
-------------------------
STATUS CODE:
{StatusCode}

HEADERS:
{Headers}

BODY:
{@Body}
", response.StatusCode, response.Headers, response.Body);
        }
    }
}
