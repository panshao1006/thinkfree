using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.CustomException
{
    /// <summary>
    /// 业务异常类
    /// </summary>
    public class BusinessException : Exception
    {
        private string _error;
        private Exception _innerException;

        private BusinessErrorCode _errorCode = BusinessErrorCode.Unkown;

        public BusinessException()
        {

        }
        
        public BusinessException(string message) : base(message)
        {
            this._error = message;
        }

        public BusinessException(string message , BusinessErrorCode errorCode) : base(message)
        {
            this._error = message;

            this._errorCode = errorCode;
        }

        public BusinessException(string message, Exception innerException):base(message)
        {
            this._innerException = innerException;
            this._error = message;
        }

        public BusinessErrorCode GetErrorCode()
        {
            return _errorCode;
        }
    }
}
