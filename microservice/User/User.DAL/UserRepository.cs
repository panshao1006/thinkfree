using Core.ORM.Dapper.Enum;
using Core.ORM.Dapper.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using User.Interface.DAL;
using User.Model;
using User.Model.Filter;

namespace User.DAL
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserModel Create(UserModel user)
        {
            var result = _dal.Insert<UserModel>(user);

            return result;
        }


        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<UserModel> GetUser(FilterBase filter)
        {
            var users = _dal.Query<UserModel>(filter);

            return users;
        }
    }
}
