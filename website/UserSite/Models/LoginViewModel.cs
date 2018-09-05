using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSite.Models
{
    public class LoginViewModel
    {
        public string Name { set; get; }

        public string Email { set; get; }

        public string Password { set; get; }

        /// <summary>
        /// 返回的地址
        /// </summary>
        public string ReturnUri { set; get; }

    }
}
