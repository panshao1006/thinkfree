using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.BLL;
using News.Interface.BLL;
using News.Model;
using News.Model.Filter;
using News.Model.ViewModel;

namespace News.API.Controllers
{
    [Route("api/newsabstract")]
    public class NewsAbstractController : ControllerBase
    {
        private INewsBusiness _service;

        public NewsAbstractController()
        {
            _service = new NewsBusiness();
        }

        [HttpGet]
        public List<NewsAbstract> Get([FromQuery]NewsFilter filterBase)
        {
            var newsList = _service.GetAbstracts(filterBase);

            return newsList;
        }
    }
}
