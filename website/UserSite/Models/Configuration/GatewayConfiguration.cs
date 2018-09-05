using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSite.Models.Configuration
{
    public class GatewayConfiguration
    {
        public string BaseHost { set; get; }

        public int BasePort { set; get; }

        public List<RequestConfiguration> Requests { set;get;}
    }
}
