using Core.ORM.Dapper.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using User.Model;
using User.Model.Filter;

namespace User.Interface.DAL
{
    public interface IUserRepository
    {
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserModel Create(UserModel user);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<UserModel> GetUser(FilterBase filter);
    }
}
