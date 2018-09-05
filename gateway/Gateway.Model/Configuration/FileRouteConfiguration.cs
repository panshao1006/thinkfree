using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.Configuration
{
    public class FileRouteConfiguration
    {
        /// <summary>
        /// 下游服务使用的请求方式，如http https
        /// </summary>
        public string DownstreamScheme { set; get; }

        /// <summary>
        /// 下流请求的路径
        /// </summary>
        public string DownstreamPathTemplate { set; get; }


        /// <summary>
        /// 下游请求方式 get post put delete
        /// </summary>
        public string DownstreamHttpMethod { set; get; }

        /// <summary>
        /// 下游请求注册的服务名称
        /// </summary>
        public string DownstreamServiceName { set; get; }


        public List<DownstreamHostInfo> DownstreamHostInfo { set; get; }


        /// <summary>
        /// 上游请求的主机信息
        /// </summary>
        public string UpstreamHost { set; get; }

        /// <summary>
        /// 上游请求的路径
        /// </summary>
        public string UpstreamPathTemplate { set; get; }


        /// <summary>
        /// 上游请求方式 get post put delete
        /// </summary>
        public string UpstreamHttpMethod { set; get; }

        /// <summary>
        /// 请求是否需要校验权限
        /// </summary>
        public bool Authentication { set; get; }
    }
}
