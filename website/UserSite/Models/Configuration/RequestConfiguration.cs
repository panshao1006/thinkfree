using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSite.Models.Configuration
{
    public class RequestConfiguration
    {
        public string Name { set; get; }

        public string Path { set; get; }

        public string Method { set; get; }

        public string Host { set; get; }

        public int Port { set; get; }

        public string Scheme { set; get; }

        public string Query { set; get; }
    }
}
