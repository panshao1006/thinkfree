using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    /// <summary>
    /// 错误信息帮助类
    /// </summary>
    public abstract class ErrorBase
    {
        protected ErrorBase(string message, ErrorCode code)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }
        public ErrorCode Code { get; private set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
