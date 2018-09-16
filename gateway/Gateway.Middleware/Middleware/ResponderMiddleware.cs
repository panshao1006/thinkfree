using Core.Log;
using Gateway.Common;
using Gateway.Common.Responder;
using Gateway.Model;
using Gateway.Model.CustomError;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    /// <summary>
    /// 最后返回中间件
    /// </summary>
    public class ResponderMiddleware : MiddlewareBase
    {
        private readonly CustomRequestDelegate _next;

        private HttpResponser _responder;

        private ICustomLogger _log;

        public ResponderMiddleware(CustomRequestDelegate next , ICustomLogger log)
        {
            _next = next;

            _responder = new HttpResponser();

            _log = log;
        }

        public async Task Invoke(DownstreamContext context)
        {
            await _next.Invoke(context);

            if (context.IsError)
            {
                //保存请求的信息
                BaseLogModel logModel = new BaseLogModel()
                {
                    Type = 1,
                    Content = $"{context.ToErrorString()} errors found in {MiddlewareName}. Setting error response for request path:{context.HttpContext.Request.Path}, request method: {context.HttpContext.Request.Method}"
                };

                _log.Error(logModel);

                SetErrorResponse(context.HttpContext, context.Errors);
            }
            else
            {
                //Logger.LogDebug("no pipeline errors, setting and returning completed response");
                await _responder.SetResponseOnHttpContext(context.HttpContext, context.DownstreamResponse);
            }
        }

        /// <summary>
        /// 返回错误的结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errors"></param>
        private void SetErrorResponse(HttpContext context, List<ErrorBase> errors)
        {
            var statusCode = new HttpStatusCodeMapper().Map(errors);
            _responder.SetErrorResponseOnContext(context, statusCode);
        }
    }
}
