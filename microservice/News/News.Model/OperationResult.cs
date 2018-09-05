using System;
using System.Collections.Generic;
using System.Text;

namespace News.Model
{
    public class OperationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { set; get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> Messages { set; get; }


        public OperationResult()
        {
            Messages = new List<string>();
        }
    }
}
