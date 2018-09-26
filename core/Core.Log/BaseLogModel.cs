using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Log
{
    public class BaseLogModel
    {
        /// <summary>
        /// 跟踪id
        /// </summary>
        public string TrackId { set; get; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 错误类型 1业务日志，4系统异常
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { set; get; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { set; get; }
    }
}
