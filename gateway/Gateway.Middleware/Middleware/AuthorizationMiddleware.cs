using Core.Common.JWTIdentity;
using Gateway.Common;
using Gateway.Model;
using Gateway.Model.CustomError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    /// <summary>
    /// 授权的中间件
    /// </summary>
    public class AuthorizationMiddleware : MiddlewareBase
    {
        private readonly CustomRequestDelegate _next;

        public AuthorizationMiddleware(CustomRequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(DownstreamContext context)
        {
            //如果不需要校验权限
            if (!context.DownstreamRoute.Authentication)
            {
                await _next.Invoke(context);
            }
            else
            {
                //获取token
                string token = GetToken(context.HttpContext);

                if (string.IsNullOrWhiteSpace(token) || !ValidateToken(token))
                {
                    SetPipelineError(context, new AuthorizationError());
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
        }


        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private string GetToken(HttpContext httpContext)
        {
            StringValues values;

            if (!httpContext.Request.Headers.TryGetValue("token", out values))
            {
                return null;
            }

            return values.ToString();
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool ValidateToken(string token)
        {
            JWTIdentityWarpper jWTIdentityWarpper = new JWTIdentityWarpper();
            return jWTIdentityWarpper.Validate(token);
        }

    }
}
