using Core.Common.ConfigManager;
using Gateway.Common;
using Gateway.Common.Configuration;
using Gateway.Common.Route;
using Gateway.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    /// <summary>
    /// 路由查找中间件，根据上游信息初始化下游信息
    /// </summary>
    public class RouteFinderMiddleware: MiddlewareBase
    {
        private readonly CustomRequestDelegate _next;

        public RouteFinderMiddleware(CustomRequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(DownstreamContext context)
        {
            //下游路由信息的匹配
            MapDownstreamContextAsync(context);

            //没有错误，才进入下一个中间件
            if(!context.IsError)
            {
                await _next.Invoke(context);
            }
        }


        /// <summary>
        /// 初始化路由信息
        /// </summary>
        /// <param name="context"></param>
        private void MapDownstreamContextAsync(DownstreamContext context)
        {
            //1初始化配置文件
            context.Configuration = InternalConfigurationHelper.Instance().GetConfiguration();

            //2初始化下游请求对象
            DownstreamRequestMapper requestMapper = new DownstreamRequestMapper(context.HttpContext.Request);

           var downstreamRequest = requestMapper.Create();

            //如果有错误，把错误信息加载到context
            if (requestMapper.IsError)
            {
                SetPipelineError(context, requestMapper.GetError());
                return;
            }

            context.DownstreamRequest = downstreamRequest;

            //3查找下游请求路由
            DownstreamRouteFinder downstreamRouteFinder = new DownstreamRouteFinder();

            var downstreamRoute = downstreamRouteFinder.Get(context);

            if (downstreamRouteFinder.IsError)
            {
                SetPipelineError(context, downstreamRouteFinder.GetError());
                return;
            }

            context.DownstreamRoute = downstreamRoute;

            //4下游请求改写
            ResetDownsteamRequest(context);

        }


        /// <summary>
        /// 重新设置下游请求
        /// </summary>
        /// <param name="context"></param>
        private void ResetDownsteamRequest(DownstreamContext context)
        {
            var downstreamRequest = context.DownstreamRequest;

            var downstreamRoute = context.DownstreamRoute;

            downstreamRequest.Host = downstreamRoute.Host;

            downstreamRequest.Port = downstreamRoute.Port;

            downstreamRequest.Scheme = downstreamRoute.Scheme;

            downstreamRequest.AbsolutePath = downstreamRoute.PathTemplate;

        }
    }
}
