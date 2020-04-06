using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using ELearningV1._3._1.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace ELearningV1._3._1.Managers
{
    public class CookieManager : ICookieManager
    {

        private IConfiguration _config;

        public CookieManager(IConfiguration configuration)
        {
            _config = configuration;
        }
        public CookieOptions CreateCookieOption(int? expireTime = 1400)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            return option;
        }

        public string GenerateJSONWebToken(User userInfo, int? expireTime = 1400)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("id", userInfo.Id ),
                new Claim("user", userInfo.UserName),
                new Claim("user_role", userInfo.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JWToken = new JwtSecurityToken(
             issuer: "http://localhost:4200/",
             audience: "http://localhost:4200/",
             claims: claims,
             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
             expires: new DateTimeOffset(DateTime.Now.AddMinutes(expireTime.Value)).DateTime,
             signingCredentials: credentials
         );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return token;
        }

        public string GetRoleFromToken(string header)
        {
            var token = header.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS.Claims.First(c => c.Type.Equals("user_role")).Value;
        }

    }
}
