using System;
using System.Collections.Generic;
using System.Text;

namespace User.Model.ViewModel
{
    public class LoginUserViewModel
    {
        public string Id { set; get; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string Token { set; get; }

        public string ReturnUri { set; get; }
    }
}
