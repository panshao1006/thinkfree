using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Log.BLL;
using Log.Interface.BLL;
using Log.Model;
using Log.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Log.API.Controllers
{
    [Route("api/log")]
    public class LogController : BaseController
    {
        private ILogBusiness _service;

        public LogController(ILogBusiness service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public OperatorResult Post([FromBody]LogModel log)
        {
            OperatorResult result = _service.Add(log);

            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
