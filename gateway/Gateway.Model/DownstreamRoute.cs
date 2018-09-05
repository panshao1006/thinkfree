using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model
{
    /// <summary>
    /// 下游路由对象
    /// </summary>
    public class DownstreamRoute
    { 
        /// <summary>
        /// 下游协议方式，http https
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// 下游请求主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 下游请求端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 下流请求的路径
        /// </summary>
        public string PathTemplate { set; get; }

        /// <summary>
        /// 查询字符串
        /// </summary>
        public string QueryString { set; get; }

        /// <summary>
        /// 是否需要校验权限
        /// </summary>
        public bool Authentication { set; get; }
    }
}
