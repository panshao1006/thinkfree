using Gateway.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model
{
    /// <summary>
    /// 配置文件类
    /// </summary>
    public class FileConfiguration
    {
        public List<FileRouteConfiguration> Routes { get; set; }
    }

    /// <summary>
    /// 下游服务的请求地址
    /// </summary>
    public class DownstreamHostInfo
    {
        public string IP { set; get; }

        public string Port { set; get; }
    }
}
