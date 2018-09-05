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
    public class NewsAbstractController : BaseController
    {
        private string _newsAbstractServerKey = "NewsAbstract";

        public NewsAbstractController(IOptions<GatewayConfiguration> option) : base(option)
        {

        }
        

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Get()
        {
            var result = GetResponse(_newsAbstractServerKey);

            return Json(result.Result);
        }
    }
}
