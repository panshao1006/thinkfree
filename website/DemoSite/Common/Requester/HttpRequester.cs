using DemoSite.Models;
using DemoSite.Models.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoSite.Common.Requester
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

            return httpRequestMessage;

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
