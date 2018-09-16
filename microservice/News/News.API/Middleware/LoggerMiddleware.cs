using Core.Common;
using Core.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using News.Model.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ICustomLogger _log;

        private LogOptions _options;

        public LoggerMiddleware(RequestDelegate next , ICustomLogger log , IOptions<LogOptions> options)
        {
            this.next = next;

            this._log = log;

            this._options = options.Value;
        }


        /// <summary>
        /// 错误中间件委托方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //保存请求的信息
            BaseLogModel logModel = new BaseLogModel()
            {
                Type = 1,
                Content = GetLogConent(context),
                ServiceName = _options.ServiceName
            };

            _log.Info<BaseLogModel>(logModel);

            await next(context);

        }

        /// <summary>
        /// 获取请求的日志内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetLogConent(HttpContext context)
        {
            string url = context.Request.GetAbsoluteUri();
            string method = context.Request.Method;

            string requestBody =  new StreamReader(context.Request.Body).ReadToEndAsync().Result;

            string dateTime = DateTime.UtcNow.ToString();

            string requestIP = context.Request.Headers["X-Original-For"].FirstOrDefault();

            string result = JsonConvert.SerializeObject(new { requestUri = url, method = method, requestContent = requestBody, requestIP = requestIP , requestDateTime = dateTime });

            return result;
        }
    }
}
