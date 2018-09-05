using Core.Common.JWTIdentity;
using System;
using System.Collections.Generic;
using System.Text;
using User.Model;

namespace User.BLL
{
    public class IdentityBusiness
    {
        private JWTIdentityWarpper jwtIdentityWarpper = new JWTIdentityWarpper();


        /// <summary>
        /// 获取一个jwttoken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetToken(UserModel user)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();

            claims.Add("Id" , user.Id);

            claims.Add("Name", user.Name);

            claims.Add("Email", user.Email);
            
            return jwtIdentityWarpper.Create(claims);
        }


        /// <summary>
        /// 验证一个jwttoken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ValidateToken(string token)
        {
            return jwtIdentityWarpper.Validate(token);
        }
    }
}
