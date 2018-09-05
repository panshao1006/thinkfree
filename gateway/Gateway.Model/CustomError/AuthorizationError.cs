using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Model.CustomError
{
    public class AuthorizationError : ErrorBase
    {
        public AuthorizationError()
            : base($"not authorization", ErrorCode.UnauthorizedError)
        {
        }
    }
}
