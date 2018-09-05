using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public enum ErrorCode
    { 
        UnmappableRequestError=1010,
        UnableToFindDownstreamRouteError = 1020,
        RequestTimedOutError = 2010,
        UnableToCompleteRequestError=3010,
        UnauthenticatedError = 4010,
        UnauthorizedError = 4020,
        ConfigurationError = 5010,
    }
}
