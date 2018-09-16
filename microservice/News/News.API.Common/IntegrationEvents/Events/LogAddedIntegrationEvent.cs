using Core.EventBus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.Common.IntegrationEvents.Events
{
    public class LogAddedIntegrationEvent: IntegrationEvent
    {
        public string LogContent { set; get; }

        public int LogType { set; get; }
    }
}
