using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    /// <summary>
    /// 配置文件错误
    /// </summary>
    public class ConfigurationError : ErrorBase
    {
        public ConfigurationError(string upstreamPath)
              : base($"没有找到上游请求的下游配置信息:{upstreamPath}", ErrorCode.ConfigurationError)
        {

        }
    }
}
