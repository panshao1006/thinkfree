using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Log
{
    public class DefaultLogger : ICustomLogger
    {
        /// <summary>
        /// 日志服务地址
        /// </summary>
        private string _host;

        private int _port;

        private string _scheme;

        private HttpClient _client;

        public DefaultLogger(string host, string port, string scheme)
        {
            _host = host;

            int.TryParse(port , out _port);

            _scheme = scheme;

            _client = new HttpClient();

        }


        public Task Fatal<T>(T log)
        {
            throw new NotImplementedException();
        }

        public Task Error<T>(T log)
        {
            throw new NotImplementedException();
        }

        public async Task Info<T>(T log)
        {
            HttpRequestMessage httpRequestMessage = GetRequestMessage<T>("post", log);

            await _client.SendAsync(httpRequestMessage);
        }



        private HttpRequestMessage GetRequestMessage<T>(string method, T reqeustValue)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();


            if (method.ToLower() == "post")
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(reqeustValue), Encoding.UTF8, "application/json");

                httpRequestMessage.Content = content;
            }
            else if (method.ToLower() == "get")
            {
                var keyValues = GetQueryContentFormObject(reqeustValue);
                //get请求
                HttpContent content = new FormUrlEncodedContent(keyValues);

                content.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                httpRequestMessage.Content = content;
            }

            var uri = new UriBuilder
            {
                Port = _port,
                Host = _host,
                Path = "api/log",
                Scheme = _scheme
            };

            httpRequestMessage.RequestUri = uri.Uri;

            httpRequestMessage.Method = new HttpMethod(method);

            return httpRequestMessage;
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

            foreach (var propertyInfo in propertyInfos)
            {
                string fieldName = propertyInfo.Name;
                string fieldValue = Convert.ToString(propertyInfo.GetValue(value));

                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    continue;
                }

                keyValues.Add(fieldName, fieldValue);
            }

            return keyValues;
        }
    }
}
