using AdminsSite.Common.Requester;
using AdminsSite.Models;
using AdminsSite.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdminsSite.Controllers
{
    public class BaseController: Controller
    {
        private GatewayConfiguration _configuration;

        private HttpRequester _requester;

        public BaseController(IOptions<GatewayConfiguration> option)
        {
            _configuration = option.Value;

            _requester = new HttpRequester();
        }

        /// <summary>
        /// 获取api请求的信息
        /// </summary>
        /// <param name="requestName"></param>
        /// <returns></returns>
        public RequestConfiguration GetRequestInfo(string requestName)
        {
            if(_configuration == null || _configuration.Requests == null || _configuration.Requests.Count == 0)
            {
                throw new Exception("没有gateway的配置信息");
            }

            RequestConfiguration requestConfiguration = _configuration.Requests.First(x => string.Compare(x.Name, requestName, true) == 0);

            if (requestConfiguration != null)
            {
                requestConfiguration.Host = requestConfiguration.Host ?? _configuration.BaseHost;
                requestConfiguration.Port = requestConfiguration.Port == 0 ? _configuration.BasePort: requestConfiguration.Port;
            }

            return requestConfiguration;
        }

        /// <summary>
        /// 获取api返回结果
        /// </summary>
        /// <param name="romoteServerName"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<dynamic>> GetResponse(string romoteServerName)
        {
            var requestConfiguration = GetRequestInfo(romoteServerName);
            var responseResult = await _requester.GetRepsonse(requestConfiguration, HttpContext.Request);

            string resultString = string.Empty;

            var result = new RepsonseResult<dynamic>()
            {
                Errors = responseResult.Errors
            };


            if (responseResult.Data.IsSuccessStatusCode && responseResult.Data.Content != null)
            {
                resultString = await responseResult.Data.Content.ReadAsStringAsync();

                result.IsSuccess = true;
                result.Data = JsonConvert.DeserializeObject(resultString);
            }

            return result;
        }

        /// <summary>
        /// 获取api返回结果
        /// </summary>
        /// <param name="romoteServerName"></param>
        /// <returns></returns>
        public async Task<RepsonseResult<dynamic>> GetResponse<T>(string romoteServerName , T contentValue)
        {
            var requestConfiguration = GetRequestInfo(romoteServerName);
            var responseResult = await _requester.GetRepsonse<T>(requestConfiguration, contentValue);

            string resultString = string.Empty;

            var result = new RepsonseResult<dynamic>()
            {
                Errors = responseResult.Errors
            };


            if (responseResult.Data.IsSuccessStatusCode && responseResult.Data.Content != null)
            {
                resultString = await responseResult.Data.Content.ReadAsStringAsync();

                result.IsSuccess = true;
                result.Data = JsonConvert.DeserializeObject(resultString);
            }

            return result;
        }
    }
}
