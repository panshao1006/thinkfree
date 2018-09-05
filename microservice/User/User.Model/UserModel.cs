using System;
using System.Collections.Generic;
using System.Text;

namespace User.Model
{
    public class UserModel : BaseModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { set; get; }


        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        public UserModel() : base("thinkfree_user")
        {

        }
    }
}
