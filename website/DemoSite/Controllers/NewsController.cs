using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoSite.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoSite.Controllers
{
    public class NewsController : BaseController
    {
        private string _newsServerKey = "News";

        public NewsController(IOptions<GatewayConfiguration> option) : base(option)
        {

        }

        public IActionResult Index(string id)
        {
            return View();
        }

        /// <summary>
        /// 获取news对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetNews(string id)
        {
            var response = GetResponse(_newsServerKey);

            return Json(response.Result);
        }
        
    }
}
