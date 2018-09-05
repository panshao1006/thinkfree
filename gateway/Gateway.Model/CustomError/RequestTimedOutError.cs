using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public class RequestTimedOutError : ErrorBase
    {
        public RequestTimedOutError(Exception exception)
            : base($"Timeout making http request, exception: {exception}", ErrorCode.RequestTimedOutError)
        {
        }
    }
}
