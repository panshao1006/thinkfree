using Core.ORM.Dapper.Enum;
using Core.ORM.Dapper.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using User.DAL;
using User.Interface.BLL;
using User.Interface.DAL;
using User.Model;
using User.Model.Filter;
using User.Model.ViewModel;

namespace User.BLL
{
    public class UserBusiness : BusinessBase<UserFilter>, IUserBusiness
    {
        private IUserRepository _dal;


        public UserBusiness()
        {
            _dal = new UserRepository();
        }

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserViewModel Create(UserModel user)
        {
            var resultUser = _dal.Create(user);

            if (resultUser == null)
            {
                return null;
            }

            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone
            };

            return userViewModel;
        }


        /// <summary>
        /// 获取一个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel GetUserById(string id)
        {
            FilterBase filter = new FilterBase();
            filter.Equal("id", id);

            var users = _dal.GetUser(filter);

            UserModel result = users != null ? users.First() : null;

            return result;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<UserModel> GetUser(UserFilter filter)
        {
            FilterBase dapperFilter = ConvertFilter(filter);

            var users = _dal.GetUser(dapperFilter);

            //逻辑处理
            if (filter.FilterType == 1)
            {
                users.ForEach(x =>
                {
                    x.Password = null;
                });
            }

            return users;
        }


        /// <summary>
        /// 自定义的filter转换
        /// </summary>
        /// <param name="dapperFilter"></param>
        /// <param name="filter"></param>
        protected override void CustomConvert(FilterBase dapperFilter, UserFilter filter)
        {
            //登录
            if (filter.FilterType == 1)
            {

                List<FilterCondition> segmentConditions1 = new List<FilterCondition>()
                {
                    new FilterCondition("Name" , LogicType.Equal , OperatorType.AND , filter.Name),
                    new FilterCondition("Password" , LogicType.Equal, OperatorType.AND , filter.Password)
                };

                dapperFilter.AddFilterSegment(segmentConditions1, OperatorType.AND);

                List<FilterCondition> segmentConditions2 = new List<FilterCondition>()
                {
                    new FilterCondition("Email" , LogicType.Equal , OperatorType.AND , filter.Name),
                    new FilterCondition("Password" , LogicType.Equal, OperatorType.AND , filter.Password)
                };

                dapperFilter.AddFilterSegment(segmentConditions2, OperatorType.OR);

            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public LoginUserViewModel Login(UserFilter filter)
        {
            var users = GetUser(filter);

            if (users == null || users.Count > 1)
            {
                return null;
            }

            var user = users.FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            //生成一个token
            IdentityBusiness identityBusiness = new IdentityBusiness();

            string token = identityBusiness.GetToken(user);


            LoginUserViewModel viewModel = new LoginUserViewModel()
            {
                Id = user.Id,
                Email =user.Email,
                Token = token,
                ReturnUri = filter.ReturnUri
            };

            return viewModel;
        }


    }
}
