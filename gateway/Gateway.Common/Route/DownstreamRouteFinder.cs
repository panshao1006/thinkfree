using Gateway.Model;
using Gateway.Model.Configuration;
using Gateway.Model.CustomError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway.Common.Route
{
    /// <summary>
    /// 下游请求查找
    /// </summary>
    public class DownstreamRouteFinder
    {
        private List<ErrorBase> _errors;

        public bool IsError => _errors.Count > 0;

        public DownstreamRouteFinder()
        {
            _errors = new List<ErrorBase>();
        }

        /// <summary>
        /// 根据上游请求找到下游请求对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public DownstreamRoute Get(DownstreamContext context)
        {
            var configuration = context.Configuration;

            var httpContextRequest = context.HttpContext.Request;

            var upstreamUrlPath = httpContextRequest.Path.ToString();

            var upstreamQueryString = httpContextRequest.QueryString.ToString();

            var upstreamHost = httpContextRequest.Headers["Host"];


            var configRoutes = configuration.Routes;

            if (configRoutes == null || configRoutes.Count == 0)
            {
                var error = new ConfigurationError(upstreamUrlPath);
                _errors.Add(error);

                return null;
            }

            var upstreamUri = $"{upstreamHost}{upstreamUrlPath}";

            //从配置文件里面查找进行
            var configRoute = configRoutes.FirstOrDefault(x => x.UpstreamHost + x.UpstreamPathTemplate == upstreamUri);

            if (configRoute == null)
            {
                var error = new ConfigurationError(upstreamUrlPath);
                _errors.Add(error);

                return null;
            }

            DownstreamRoute route = GetDownstreamRoute(configRoute , upstreamQueryString);

            return route;
        }

        /// <summary>
        /// 获取下游请求信息
        /// </summary>
        /// <param name="routeConfiguration"></param>
        /// <param name="upstreamQueryString">上游请求的查询字符串</param>
        /// <returns></returns>
        private DownstreamRoute GetDownstreamRoute(FileRouteConfiguration routeConfiguration , string upstreamQueryString)
        {
            DownstreamRoute downstreamRoute = new DownstreamRoute();

            var downstreamHostInfo = GetDownstreamHostString(routeConfiguration.DownstreamHostInfo);

            downstreamRoute.Host = downstreamHostInfo.IP;

            downstreamRoute.Port = int.Parse(downstreamHostInfo.Port);

            downstreamRoute.Scheme = routeConfiguration.DownstreamScheme;

            downstreamRoute.PathTemplate = UriMapper.GetRestfullUri(routeConfiguration.DownstreamPathTemplate, upstreamQueryString);

            downstreamRoute.QueryString = upstreamQueryString;

            downstreamRoute.Authentication = routeConfiguration.Authentication;

            return downstreamRoute;
        }


        private DownstreamHostInfo GetDownstreamHostString(List<DownstreamHostInfo> downstreamHostInfos)
        {
            if (downstreamHostInfos == null || downstreamHostInfos.Count == 0)
            {
                throw new Exception("没有配置下游请求的DownstreamHostInfo信息");
            }

            //这里要考虑做负载均衡
            var downstreamHostInfo = downstreamHostInfos.First();

            return downstreamHostInfo;
        }


        public List<ErrorBase> GetError()
        {
            return _errors;
        }
    }
}
