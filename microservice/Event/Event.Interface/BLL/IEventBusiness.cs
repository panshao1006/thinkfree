using Event.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Event.Interface.BLL
{
    public interface IEventBusiness
    {
        /// <summary>
        /// 新增一个事件
        /// </summary>
        /// <param name="eventModel"></param>
        void AddEvent(EventModel eventModel);


        /// <summary>
        /// 查询一个事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EventModel GetEvent(string id);

        /// <summary>
        /// 发布一个事件
        /// </summary>
        /// <param name="id"></param>
        void PublishEvent(string id);
        
        
    }
}
