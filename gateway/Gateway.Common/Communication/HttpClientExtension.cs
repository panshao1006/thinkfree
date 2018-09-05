using Gateway.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Common.Communication
{
    public class HttpClientExtension
    {
        private HttpClient _httpClient;

        /// <summary>
        /// 默认超时时间
        /// </summary>
        private TimeSpan _defaultTimeout = TimeSpan.FromSeconds(60);

        public HttpClientExtension()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            _httpClient.Timeout = _defaultTimeout;

            return _httpClient.SendAsync(request);
        }
    }
}
