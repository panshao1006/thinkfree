﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway.Model
{
    /// <summary>
    /// 路由上下文
    /// </summary>
    public class RouteContext
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


        public List<DownstreamHostInfo> DownstreamHostInfos { set; get; }

        /// <summary>
        /// 获取下游服务的访问地址
        /// </summary>
        public string DownstreamUri
        {
            get
            {
                DownstreamHostInfo downstreamHost = DownstreamHostInfos.FirstOrDefault();

                if (downstreamHost == null)
                {
                    throw new System.Exception($"上游地址{UpstreamUri}，没有找到下游主机信息");
                }
                string downstreamUri = $"{DownstreamScheme}://{downstreamHost.IP}:{downstreamHost.Port}{DownstreamPathTemplate}";

                return downstreamUri;
            }
        }


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
        public string Authentication { set; get; }

        /// <summary>
        /// 当前请求信息
        /// </summary>
        public HttpContext HttpContext { set; get; }

        /// <summary>
        /// 获取url
        /// </summary>
        public string UpstreamUri
        {
            get
            {
                var request = HttpContext.Request;

                string host = request.Host.Value;

                string path = request.Path.Value;

                return $"{host}{path}";
            }
        }

        public RouteContext()
        {

        }

        public RouteContext(HttpContext context)
        {
            this.HttpContext = context;
        }
    }
}
