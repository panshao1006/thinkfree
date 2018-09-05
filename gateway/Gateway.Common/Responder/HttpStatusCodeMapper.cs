using Gateway.Model.CustomError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway.Common.Responder
{
    /// <summary>
    /// 内部错误码转换为httpstatuscode
    /// </summary>
    public class HttpStatusCodeMapper
    {
        public int Map(List<ErrorBase> errors)
        {
            if (errors.Any(e => e.Code == ErrorCode.UnauthenticatedError))
            {
                return 401;
            }

            if (errors.Any(e => e.Code == ErrorCode.UnauthorizedError
                //|| e.Code == ErrorCode.ClaimValueNotAuthorisedError
                //|| e.Code == ErrorCode.ScopeNotAuthorisedError
                //|| e.Code == ErrorCode.UserDoesNotHaveClaimError
                //|| e.Code == ErrorCode.CannotFindClaimError
                ))
            {
                return 403;
            }

            if (errors.Any(e => e.Code == ErrorCode.RequestTimedOutError))
            {
                return 503;
            }

            if (errors.Any(e => e.Code == ErrorCode.UnableToFindDownstreamRouteError))
            {
                return 404;
            }

            if (errors.Any(e => e.Code == ErrorCode.UnableToCompleteRequestError))
            {
                return 500;
            }

            return 404;
        }
    }
}
