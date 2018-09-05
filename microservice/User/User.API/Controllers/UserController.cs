using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.BLL;
using User.Interface.BLL;
using User.Model;
using User.Model.Filter;
using User.Model.ViewModel;

namespace User.API.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private IUserBusiness _services = new UserBusiness();

        [HttpGet]
        public List<UserModel> Get(UserFilter filter)
        {
            List<UserModel> users = _services.GetUser(filter);

            return users;
        }

        
        // GET api/values/5
        [HttpGet("{id}")]
        public UserModel Get(string id)
        {
            UserModel result = _services.GetUserById(id);

            return result;
        }

        [HttpPost]
        public UserViewModel Post([FromBody]UserModel user)
        {
            UserViewModel result = _services.Create(user);

            if(result == null)
            {
                throw new ArgumentNullException("创建用户失败");
            }

            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public LoginUserViewModel Login([FromBody]UserFilter filter)
        {
            LoginUserViewModel result = _services.Login(filter);

            return result;
        }
    }
}