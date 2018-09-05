using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public class UnableToCompleteRequestError : ErrorBase
    {
        public UnableToCompleteRequestError(Exception exception)
            : base($"Error making http request, exception: {exception}", ErrorCode.UnableToCompleteRequestError)
        {
        }
    }
}
