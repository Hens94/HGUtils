using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
    }
}
