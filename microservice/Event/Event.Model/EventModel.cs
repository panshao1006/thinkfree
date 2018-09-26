using System;

namespace Event.Model
{
    public class EventModel: BaseModel
    {
        public EventModel() : base("thinkfree_event")
        {
            ;
        }

        public DateTime CreateDate { set; get; }

        public string EventType { set; get; }

        public string EventContent { set; get; }

        public int Status { set; get; } 
    }
}
