using Event.Model;
using System;

namespace Event.DAL
{
    public class EventRepository : BaseRepository
    {
        public void AddEvent(EventModel eventModel)
        {
            _dal.Insert<EventModel>(eventModel);
        }

        public void UpdateEvent(EventModel eventModel)
        {
            _dal.Update<EventModel>(eventModel);
        }

        //public EventModel GetEvent(string id)
        //{
        //    _dal.Query<EventModel>();
        //}
    }
}
