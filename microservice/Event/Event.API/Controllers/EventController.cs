using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Event.Model;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Controllers
{
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        [HttpGet("{id}")]
        public List<EventModel> Get(int id)
        {
            List<EventModel> events = new List<EventModel>();
            return events;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] EventModel @event)
        {
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
