using AdminsSite.Common.Requester;
using AdminsSite.Models.Configuration;
using Core.Common;
using Core.Common.ConfigManager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminsSite.Common.Middleware
{
    /// <summary>
    /// 授权的中间件
    /// </summary>
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        private GatewayConfiguration _configuration;

        public AuthorizationMiddleware(RequestDelegate next , GatewayConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            //获取cookie中的token
            string token;

            context.Request.Cookies.TryGetValue("Token", out token);

            //如果没有token,肯定是不成功的
            if (string.IsNullOrWhiteSpace(token))
            {
                RedirectLoginSite(context);

                return;
            }

            //校验token是否有效
            HttpRequester requester = new HttpRequester(_configuration);

            context.Request.Headers.TryAdd("Token", token);

            var response = await requester.GetResponse("Auth_Validate", context.Request);

            if (response.Data!=null && response.Data.IsSuccessStatusCode)
            {
                await _next.Invoke(context);
            }
            else
            {
                RedirectLoginSite(context);
            }
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="context"></param>
        private void RedirectLoginSite(HttpContext context)
        {
            string loaction = ConfigurtaionManager.AppSettings("LoginUri");

            string currentRequestUri = context.Request.GetAbsoluteUri();

            string fullUri = $"{loaction}?returnuri={currentRequestUri}";

            context.Response.Redirect(fullUri);
        }
    }
}
