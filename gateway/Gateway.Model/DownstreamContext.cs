using Gateway.Model.CustomError;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model
{
    /// <summary>
    /// 下游请求上下文
    /// </summary>
    public class DownstreamContext
    {
        public DownstreamContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
            Errors = new List<ErrorBase>();
        }

        public HttpContext HttpContext { set; get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<ErrorBase> Errors { get; }

        /// <summary>
        /// 下游请求
        /// </summary>
        public DownstreamRequest DownstreamRequest { set; get; }

        /// <summary>
        /// 下游请求结果
        /// </summary>
        public DownstreamResponse DownstreamResponse { set; get; }

        /// <summary>
        /// 查找到的下游请求路由
        /// </summary>
        public DownstreamRoute DownstreamRoute { set; get; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public FileConfiguration Configuration { set; get; }

        /// <summary>
        /// 是否存在错误
        /// </summary>
        public bool IsError => Errors.Count > 0;

        public string ToErrorString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(ErrorBase error in Errors)
            {
                builder.AppendLine(error.ToString());
            }

            return builder.ToString();
        }
    }
}
