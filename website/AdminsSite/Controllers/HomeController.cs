using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminsSite.Models;
using AdminsSite.Models.Configuration;
using Microsoft.Extensions.Options;

namespace AdminsSite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IOptions<GatewayConfiguration> option) : base(option)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
