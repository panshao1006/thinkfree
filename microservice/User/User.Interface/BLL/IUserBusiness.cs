using System;
using System.Collections.Generic;
using System.Text;
using User.Model;
using User.Model.Filter;
using User.Model.ViewModel;

namespace User.Interface.BLL
{
    public interface IUserBusiness
    {
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserViewModel Create(UserModel user);

        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUserById(string id);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<UserModel> GetUser(UserFilter filter);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        LoginUserViewModel Login(UserFilter filter);
    }
}
