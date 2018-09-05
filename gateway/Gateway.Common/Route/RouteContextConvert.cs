using Gateway.Common.Configuration;
using Gateway.Model;
using Gateway.Model.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gateway.Common.Route
{
    /// <summary>
    /// 路由帮助类
    /// </summary>
    public class RouteContextConvert
    {
        /// <summary>
        /// 从配置文件中读取信息，并转换为路由上下文
        /// </summary>
        /// <param name="routeContext"></param>
        /// <returns></returns>
        public static RouteContext Convert(DownstreamContext routeContext)
        {
            FileConfiguration fileConfiguration = InternalConfigurationHelper.Instance().GetConfiguration();

            string upstreamUri = routeContext.UpstreamUri;

            var configRoutes = fileConfiguration.Routes;

            if(configRoutes == null || configRoutes.Count == 0)
            {
                routeContext = null;
                return routeContext;
            }

            //从配置文件里面查找进行
            var configRoute = configRoutes.FirstOrDefault(x => x.UpstreamHost + x.UpstreamPathTemplate == upstreamUri);

            if(configRoute == null)
            {
                routeContext = null;
                return routeContext;
            }

            Copy(routeContext, configRoute);

            return routeContext;
        }


        /// <summary>
        /// 从配置的路由中复制值
        /// </summary>
        /// <param name="configRoute"></param>
        /// <returns></returns>
        public static RouteContext Copy(RouteContext routeContext , FileRouteConfiguration configRoute)
        {
            routeContext.Authentication = configRoute.Authentication;
            routeContext.DownstreamScheme = configRoute.DownstreamScheme;
            routeContext.DownstreamPathTemplate = configRoute.DownstreamPathTemplate;
            routeContext.DownstreamHttpMethod = configRoute.DownstreamHttpMethod;
            routeContext.DownstreamServiceName = configRoute.DownstreamServiceName;
            routeContext.DownstreamHostInfos = configRoute.DownstreamHostInfo;
            routeContext.UpstreamHost = configRoute.UpstreamHost;
            routeContext.UpstreamPathTemplate = configRoute.UpstreamPathTemplate;
            routeContext.UpstreamHttpMethod = configRoute.UpstreamHttpMethod;

            return routeContext;
        }
    }
}
