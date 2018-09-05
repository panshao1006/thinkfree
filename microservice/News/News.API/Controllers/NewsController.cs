using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.BLL;
using News.Interface.BLL;
using News.Model;
using News.Model.Filter;

namespace News.API.Controllers
{
    [Route("api/news")]
    public class NewsController : Controller
    {
        private INewsBusiness _service;

        public NewsController()
        {
            _service = new NewsBusiness();
        }

        [HttpGet]
        public List<NewsModel> Get([FromQuery]NewsFilter filter)
        {
            var newsList = _service.GetNews(filter);

            return newsList;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public NewsModel Get(string id)
        {
            var newsList = _service.GetNewsById(id);

            return newsList;
        }

        // POST api/values
        [HttpPost]
        public OperationResult Post([FromBody]NewsModel news)
        {
            var result = _service.AddNews(news);

            return result;
        }

        // PUT api/values/5
        [HttpPut]
        public OperationResult Put([FromBody]NewsModel news)
        {
            var result = _service.Update(news);

            return result;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public OperationResult Delete(string id)
        {
            var result = _service.Delete(id);

            return result;
        }
    }
}
