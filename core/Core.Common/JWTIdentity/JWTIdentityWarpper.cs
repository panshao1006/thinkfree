using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Common.JWTIdentity
{
    /// <summary>
    /// jwt认证封装
    /// </summary>
    public class JWTIdentityWarpper
    {
        /// <summary>
        /// 创建一个jwttoken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Create(Dictionary<string , string> claimsKeyValues)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Consts.JWTSecret);
            var authTime = DateTime.UtcNow;
            var expiresTime = authTime.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(ConvertToClaims(claimsKeyValues)),
                Expires = expiresTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="claimsKeyValues"></param>
        /// <returns></returns>
        private List<Claim> ConvertToClaims(Dictionary<string, string> claimsKeyValues)
        {
            if(claimsKeyValues == null)
            {
                throw new ArgumentNullException("claimsKeyValues 不能为空");
            }

            List<Claim> claims = new List<Claim>();

            foreach (var claimsKey in claimsKeyValues.Keys)
            {
                Claim claim = new Claim(claimsKey, claimsKeyValues[claimsKey]);

                claims.Add(claim);
            }

            return claims;
        }


        /// <summary>
        /// 验证一个jwt token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Validate(string token)
        {
            bool result = false;

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Consts.JWTSecret))
            };

            SecurityToken securityToken;

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters , out securityToken);

            result = claimsPrincipal.Identity.IsAuthenticated;

            return result;
        }
    }
}
