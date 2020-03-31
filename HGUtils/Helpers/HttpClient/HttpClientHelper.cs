using HGUtils.Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace HGUtils.Helpers.HttpClient
{
    public static class HttpClientHelper
    {
        public static string AddQueryParams<T>(this string endpoint, T properties) where T : class =>
                UpdateQueryString(endpoint, q =>
                {
                    if (!(properties is null))
                    {
                        foreach (var propertyInfo in properties.GetType().GetProperties())
                        {
                            if (!(
                                propertyInfo.PropertyType.IsGenericType &&
                                propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                            {
                                var propertyValue = propertyInfo.GetValue(properties, null);
                                if (!(propertyValue is null))
                                {
                                    q.Set(propertyInfo.Name, propertyInfo.GetValue(properties, null).ToString());
                                }
                            }
                        }
                    }
                });

        public static string AddQueryParam<T>(this string endpoint, string paramName, T paramValue) =>
            UpdateQueryString(endpoint, q => q.Set(paramName, paramValue.ToString()));

        public static string AddQueryParam<T>(this string endpoint, string paramName, IEnumerable<T> paramValues)
        {
            if (!(paramValues is null) && paramName.Any())
            {
                foreach (var paramValue in paramValues)
                {
                    endpoint = endpoint.AddQueryParam(paramName, paramValue);
                }
            }

            return endpoint;
        }

        private static string UpdateQueryString(string url, Action<NameValueCollection> func)
        {
            var urlWithoutQueryString = url.Contains('?') ? url.Substring(0, url.IndexOf('?')) : url;
            var queryString = url.Contains('?') ? url.Substring(url.IndexOf('?')) : null;
            var query = HttpUtility.ParseQueryString(string.Empty);

            func(query);

            if (queryString is null)
            {
                return urlWithoutQueryString + (query.Count > 0 ? "?" : string.Empty) + query;
            }
            else
            {
                return urlWithoutQueryString + queryString + (query.Count > 0 ? "&" : string.Empty) + query;
            }
        }

        public static System.Net.Http.HttpClient AddAuthorizationHeader(this System.Net.Http.HttpClient client, string authorizationToken, string contentType = "application/json")
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

            return client;
        }

        public static void AddHttpContent<T>(this HttpRequestMessage request, T content, string contentType = "application/json")
        {
            var stringContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                contentType);

            request.Content = stringContent;
        }

        public static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content.Clone(),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        public static HttpResponseMessage Clone(this HttpResponseMessage response)
        {
            var clone = new HttpResponseMessage(response.StatusCode)
            {
                Content = response.Content.Clone(),
                Version = response.Version,
                ReasonPhrase = response.ReasonPhrase,
                RequestMessage = response.RequestMessage.Clone()
            };
            foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        private static HttpContent Clone(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();
            content.CopyToAsync(ms).Wait();
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }

        public static async Task WriteRequestAndResponseInLogsAsync(this HttpResponseMessage response)
        {
            var requestLog = await response.RequestMessage.FormatRequest();
            var responseLog = await response.FormatResponse();

            requestLog.WriteApiRequestInLog();
            responseLog.WriteApiResponseInLog();
        }

        public delegate Task<string> GetTokenDelegate(bool refreshToken);

        public static async Task<HttpResponseMessage> SendAndRefrechAuthTokenAsync(
            this System.Net.Http.HttpClient httpClient,
            HttpRequestMessage httpRequest,
            GetTokenDelegate getToken = null,
            bool reuseAuthToken = false,
            int refreshTokenRetry = 1,
            bool writeInLogs = false,
            int refreshTokenRetryCount = 1,
            bool refreshToken = false)
        {
            if (getToken is null) return await httpClient.SendAsync(httpRequest);

            var request = httpRequest;

            if (refreshToken)
            {
                request = httpRequest.Clone();
            }

            var response = await httpClient
                .AddAuthorizationHeader(await getToken(refreshToken))
                .SendAsync(request);

            if (reuseAuthToken && response.StatusCode.Equals(HttpStatusCode.Unauthorized) && refreshTokenRetryCount <= refreshTokenRetry)
            {
                response = await httpClient.SendAndRefrechAuthTokenAsync(
                    request,
                    getToken,
                    reuseAuthToken,
                    refreshTokenRetry,
                    writeInLogs,
                    refreshTokenRetryCount + 1,
                    true);
            }

            if (writeInLogs) await response.WriteRequestAndResponseInLogsAsync();

            return response;
        }
    }
}
