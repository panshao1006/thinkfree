using AdminsSite.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminsSite.Common.Middleware
{
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UserAuthorization(this IApplicationBuilder builder)
        {
            var configuration = builder.ApplicationServices.GetService<IOptions<GatewayConfiguration>>();

            if (configuration.Value == null)
            {
                throw new Exception("无法读取配置文件");
            }

            return builder.UseMiddleware<AuthorizationMiddleware>(configuration.Value);
        }
    }
}
