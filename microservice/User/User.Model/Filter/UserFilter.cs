using System;
using System.Collections.Generic;
using System.Text;

namespace User.Model.Filter
{
    public class UserFilter : BaseFilter
    {
        /// <summary>
        /// 邮箱登录
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// 手机登录
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// 用户名登录
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 过滤类型，1 登录
        /// </summary>
        public int FilterType { set; get; }

        /// <summary>
        /// 返回的地址
        /// </summary>
        public string ReturnUri { set; get; }
    }
}
