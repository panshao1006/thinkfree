using Core.Common;
using Core.Log;
using Gateway.Common;
using Gateway.Model;
using Gateway.Model.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    public class LoggerMiddlewate : MiddlewareBase
    {
        private readonly CustomRequestDelegate _next;

        private readonly ICustomLogger _log;


        private readonly LogOptions _logOptions;

        public LoggerMiddlewate(CustomRequestDelegate next , ICustomLogger log , IOptions<LogOptions> options)
        {
            _next = next;
            _log = log;

            _logOptions = options.Value;
        }

        public async Task Invoke(DownstreamContext context)
        {
            //保存请求的信息
            BaseLogModel logModel = new BaseLogModel()
            {
                Type = 1,
                Content = GetLogConent(context.HttpContext),
                ServiceName = _logOptions.ServiceName
            };

            _log.Info<BaseLogModel>(logModel);

            await _next(context);
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

            string requestBody = new StreamReader(context.Request.Body).ReadToEndAsync().Result;

            string dateTime = DateTime.UtcNow.ToString();

            string requestIP = context.Request.Headers["X-Original-For"].FirstOrDefault();

            string result = JsonConvert.SerializeObject(new { requestUri = url, method = method, requestContent = requestBody, requestIP = requestIP, requestDateTime = dateTime });

            return result;
        }
    }
}
