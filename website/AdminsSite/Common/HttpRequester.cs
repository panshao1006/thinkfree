using AdminsSite.Models;
using AdminsSite.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdminsSite.Common.Requester
{
    public class HttpRequester
    {
        private HttpClient _httpClient;

        private GatewayConfiguration _configuration;

        private List<string> _rightfulHeaderKey = new List<string>() { "Token" , "token" };

        public HttpRequester()
        {
            _httpClient = new HttpClient();
        }


        public HttpRequester(GatewayConfiguration configuration)
        {
            _httpClient = new HttpClient();

            _configuration = configuration;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<HttpResponseMessage>> GetResponse(HttpRequestMessage request)
        {
            var result = await _httpClient.SendAsync(request);

            

            return new RepsonseResult<HttpResponseMessage>(result);
        }


        /// <summary>
        /// 获取api请求的信息
        /// </summary>
        /// <param name="requestName"></param>
        /// <returns></returns>
        public RequestConfiguration GetRequestInfo(string requestName)
        {
            if (_configuration == null || _configuration.Requests == null || _configuration.Requests.Count == 0)
            {
                throw new Exception("没有gateway的配置信息");
            }

            RequestConfiguration requestConfiguration = _configuration.Requests.First(x => string.Compare(x.Name, requestName, true) == 0);

            if (requestConfiguration != null)
            {
                requestConfiguration.Host = requestConfiguration.Host ?? _configuration.BaseHost;
                requestConfiguration.Port = requestConfiguration.Port == 0 ? _configuration.BasePort : requestConfiguration.Port;
            }

            return requestConfiguration;
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

            var httpResponseMessage = await GetResponse(httpRequestMessage);

            return httpResponseMessage;
        }


        /// <summary>
        /// 发送请求，获取请求结果
        /// </summary>
        /// <param name="requestConfiguration"></param>
        /// <param name="originalRequest"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<HttpResponseMessage>> GetResponse(string serverName, HttpRequest originalRequest)
        {
            RequestConfiguration requestConfiguration = GetRequestInfo(serverName);

            var httpRequestMessage = await GetHttpRequestMessage(requestConfiguration, originalRequest);

            var httpResponseMessage = await GetResponse(httpRequestMessage);

            return httpResponseMessage;
        }


        public async Task<RepsonseResult<HttpResponseMessage>> GetRepsonse<T>(string serverName , T contentValue)
        {
            RequestConfiguration requestConfiguration = GetRequestInfo(serverName);

            var result = await GetRepsonse<T>(requestConfiguration, contentValue);

            return result;
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

            var httpResponseMessage = await GetResponse(httpRequestMessage);

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
            requestConfiguration.Query = requestConfiguration.Method == "get" ? originalRequest.QueryString.ToString() : "";

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
                Content = await MapContent<T>(contentValue),
                Method = MapMethod(requestConfiguration),
                RequestUri = MapUri(requestConfiguration)
            };

            MapHeaders(null, httpRequestMessage);

            return httpRequestMessage;

        }


        private void MapHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            foreach (var header in request.Headers)
            {
                if (_rightfulHeaderKey.Contains(header.Key))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
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


        private async Task<HttpContent> MapContent<T>(T contentValue)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");

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
