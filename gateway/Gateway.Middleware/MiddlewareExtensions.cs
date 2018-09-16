using System;
using Gateway.Common.Pipeline;
using Gateway.Common.Route;
using Gateway.Middleware.Middleware;
using Gateway.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Gateway.Common.Configuration;
using Core.Log;

namespace Gateway.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExtendMiddleware(this IApplicationBuilder builder)
        {
            //配置文件初始化
            InitConfiguration(builder);

            //中间件初始化
            var pipelineBuilder = new PipelineBuilder(builder.ApplicationServices);

            pipelineBuilder.InitPipeline();

            var firstDelegate = pipelineBuilder.Build();

            builder.Use(async (context, task) =>
            {
                var downstreamContext = new DownstreamContext(context);
                await firstDelegate.Invoke(downstreamContext);
            });


            return builder;
        }


        /// <summary>
        /// 初始化中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static IPipelineBuilder InitPipeline(this IPipelineBuilder builder)
        {
            var logger = builder.ApplicationServices.GetRequiredService<ICustomLogger>();

            builder.UseMiddleware<LoggerMiddlewate>(logger); 

            builder.UseMiddleware<ResponderMiddleware>(logger);

            builder.UseMiddleware<RouteFinderMiddleware>();

            builder.UseMiddleware<AuthorizationMiddleware>();

            builder.UseMiddleware<RequesterMiddleware>();

            return builder;
        }


        /// <summary>
        /// 配置文件的初始化话
        /// </summary>
        /// <param name="builder"></param>
        private static void InitConfiguration(IApplicationBuilder builder)
        {
            var routeConfig = builder.ApplicationServices.GetService<IOptions<FileConfiguration>>();

            if(routeConfig.Value == null)
            {
                throw new Exception("无法读取配置文件");
            }

            InternalConfigurationHelper.Instance(routeConfig.Value);
        }
    }
}
