using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.BLL;
using User.Interface.BLL;

namespace User.API.Controllers
{
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        private IdentityBusiness _services = new IdentityBusiness();

        /// <summary>
        /// 校验权限
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("validate")]
        [HttpPost]
        public bool Validate([FromHeader]string token)
        {
            var result = _services.ValidateToken(token);

            return result;
        }
    }
}