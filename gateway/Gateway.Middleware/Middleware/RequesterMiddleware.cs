using Gateway.Common;
using Gateway.Common.Communication;
using Gateway.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    /// <summary>
    /// 最后的请求中间件
    /// </summary>
    public class RequesterMiddleware : MiddlewareBase
    {
        private readonly CustomRequestDelegate _next;

        private HttpClientExtension _httpClient;

        public RequesterMiddleware(CustomRequestDelegate next)
        {
            _next = next;

            _httpClient = new HttpClientExtension();
        }

        public async Task Invoke(DownstreamContext context)
        {
            HttpClientRequester requester = new HttpClientRequester();

            var responseResult = await requester.GetResponse(context);

            if (requester.IsError)
            {
                SetPipelineError(context, requester.GetError());
                return;
            }

            context.DownstreamResponse = new DownstreamResponse(responseResult);
        }
    }
}
