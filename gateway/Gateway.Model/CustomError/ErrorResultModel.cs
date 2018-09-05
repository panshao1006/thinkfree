using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public class ErrorResultModel
    {
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// 错误类型
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 子代码
        /// </summary>
        public int Subcode { set; get; }

        /// <summary>
        /// 跟踪id
        /// </summary>
        public string TraceId { set; get; }
    }
}
