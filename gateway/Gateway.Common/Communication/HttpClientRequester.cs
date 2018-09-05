using Gateway.Model;
using Gateway.Model.CustomError;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Common.Communication
{
    public class HttpClientRequester
    {
        private List<ErrorBase> _errors;


        private HttpClientExtension _httpClient;


        public bool IsError => _errors.Count > 0;


        public HttpClientRequester()
        {
            _errors = new List<ErrorBase>();

            _httpClient = new HttpClientExtension();
        }

        public async Task<HttpResponseMessage> GetResponse(DownstreamContext context)
        {
            try
            {
                var response = await _httpClient.SendAsync(context.DownstreamRequest.ToHttpRequestMessage());
                return response;
            }
            catch (TimeoutException exception)
            {
                var error = new RequestTimedOutError(exception);

                _errors.Add(error);

                return null;
            }
            catch (Exception exception)
            {
                var error = new UnableToCompleteRequestError(exception);

                _errors.Add(error);

                return null;
            }
        }

        /// <summary>
        /// 获取错误结果
        /// </summary>
        /// <returns></returns>
        public List<ErrorBase> GetError()
        {
            return _errors;
        }
    }
}
