using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public class UnmappableRequestError : ErrorBase
    {
        public UnmappableRequestError(Exception exception) : base($"Error when parsing incoming request, exception: {exception}", ErrorCode.UnmappableRequestError)
        {
        }
    }
}
