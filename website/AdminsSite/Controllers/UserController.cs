using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminsSite.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminsSite.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IOptions<GatewayConfiguration> option) : base(option)
        {

        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
