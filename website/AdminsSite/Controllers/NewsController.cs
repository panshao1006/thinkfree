using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminsSite.Models;
using AdminsSite.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminsSite.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(IOptions<GatewayConfiguration> option) : base(option)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody]NewsModel news)
        {
            var response = GetResponse<NewsModel>("News_Add", news);

            return Json(response.Result);
        }


        [HttpPost]
        public IActionResult Update([FromBody]NewsModel news)
        {
            var response = GetResponse<NewsModel>("News_Update", news);

            return Json(response.Result);
        }


        public IActionResult GetNews([FromQuery]string id)
        {
            var response = GetResponse("News");

            return Json(response.Result);
        }


        public IActionResult GetAbstracts()
        {
            var response = GetResponse("News_Abstract");

            return Json(response.Result);
        }


        public IActionResult Delete(string id)
        {
            var response = GetResponse("News_Delete");

            return Json(response.Result);
        }
    }
}
