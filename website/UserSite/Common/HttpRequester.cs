using UserSite.Models;
using UserSite.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UserSite.Common.Requester
{
    public class HttpRequester
    {
        private HttpClient _httpClient;

        public HttpRequester()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<HttpResponseMessage>> GetRepense(HttpRequestMessage request)
        {
            var result = await _httpClient.SendAsync(request);

            return new RepsonseResult<HttpResponseMessage>(result);
        }

        /// <summary>
        /// 发送请求，获取请求结果
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<HttpResponseMessage>> GetRepsonse(RequestConfiguration requestConfiguration, HttpRequest originalRequest)
        {
            var httpRequestMessage = await GetHttpRequestMessage(requestConfiguration, originalRequest);

            var httpResponseMessage = await GetRepense(httpRequestMessage);

            return httpResponseMessage;
        }


        /// <summary>
        /// 发送请求，获取请求结果
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<HttpResponseMessage>> GetRepsonse<T>(RequestConfiguration requestConfiguration, T contentValue)
        {
            var httpRequestMessage = await GetHttpRequestMessage<T>(requestConfiguration, contentValue);

            var httpResponseMessage = await GetRepense(httpRequestMessage);

            return httpResponseMessage;
        }


        /// <summary>
        /// 获取请求信息
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        public async Task<HttpRequestMessage> GetHttpRequestMessage(RequestConfiguration requestConfiguration , HttpRequest originalRequest)
        {
            requestConfiguration.Query = originalRequest.QueryString.ToString();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Content = await MapContent(originalRequest),
                Method = MapMethod(requestConfiguration),
                RequestUri = MapUri(requestConfiguration)
            };

            MapHeaders(originalRequest, httpRequestMessage);

            return httpRequestMessage;

        }


        /// <summary>
        /// 获取请求信息
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        public async Task<HttpRequestMessage> GetHttpRequestMessage<T>(RequestConfiguration requestConfiguration, T contentValue)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Content = await MapContent<T>(requestConfiguration.Method , contentValue),
                Method = MapMethod(requestConfiguration),
                RequestUri = MapUri(requestConfiguration),
            };

            MapHeaders(null, httpRequestMessage);

            return httpRequestMessage;

        }


        private void MapHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            //foreach (var header in request.Headers)
            //{
            //    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            //}
        }


        private Uri MapUri(RequestConfiguration requestConfiguration)
        {
            var uri = new UriBuilder
            {
                Port = requestConfiguration.Port,
                Host = requestConfiguration.Host,
                Path = requestConfiguration.Path,
                Query = requestConfiguration.Query,
                Scheme = requestConfiguration.Scheme
            };

            return uri.Uri;
        }

        private HttpMethod MapMethod(RequestConfiguration requestConfiguration)
        {
            return new HttpMethod(requestConfiguration.Method);
        }

        private async Task<HttpContent> MapContent(HttpRequest request)
        {
            if (request.Body == null || (request.Body.CanSeek && request.Body.Length <= 0))
            {
                return null;
            }

            var content = new ByteArrayContent(await ToByteArray(request.Body));

            if (!string.IsNullOrEmpty(request.ContentType))
            {
                content.Headers
                    .TryAddWithoutValidation("Content-Type", new[] { request.ContentType });
            }

            return content;
        }


        private async Task<HttpContent> MapContent<T>(string method , T contentValue)
        {
            if (method.ToLower() == "post")
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");

                return content;
            }
            else if(method.ToLower() == "get")
            {
                var keyValues = GetQueryContentFormObject(contentValue);
                //get请求
                HttpContent content = new FormUrlEncodedContent(keyValues);

                content.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                return content;
            }

            return null;
        }

        /// <summary>
        /// 从对象中获取querycontent
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetQueryContentFormObject(object value)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            if (value == null)
            {
                return keyValues;
            }

            var propertyInfos = value.GetType().GetProperties();

            foreach(var propertyInfo in propertyInfos)
            {
                string fieldName = propertyInfo.Name;
                string fieldValue = Convert.ToString(propertyInfo.GetValue(value));

                if(string.IsNullOrWhiteSpace(fieldValue))
                {
                    continue;
                }

                keyValues.Add(fieldName, fieldValue);
            }

            return keyValues;
        }



        private async Task<byte[]> ToByteArray(Stream stream)
        {
            using (stream)
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    return memStream.ToArray();
                }
            }
        }
    }
}
