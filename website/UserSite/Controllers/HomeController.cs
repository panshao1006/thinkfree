using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserSite.Models;
using UserSite.Models.Configuration;

namespace UserSite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IOptions<GatewayConfiguration> option):base(option)
        {
          
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginViewModel login)
        {
            if(login == null)
            {
                throw new ArgumentNullException("login 不能为空");
            }

            var response = GetResponse<LoginViewModel>("User_Login", login);

            var responseResult = response.Result;

            return Json(responseResult);

        }
    }
}
