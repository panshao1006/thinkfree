using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoSite.Models;
using Microsoft.Extensions.Options;
using DemoSite.Models.Configuration;

namespace DemoSite.Controllers
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
