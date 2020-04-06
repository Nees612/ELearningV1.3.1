using ELearningV1._3._1.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearningV1._3._1.Interfaces
{
    public interface ICookieManager
    {
        CookieOptions CreateCookieOption(int? expireTime = 1400);
        string GenerateJSONWebToken(User userInfo, int? expireTime = 1400);
        string GetRoleFromToken(string token);
    }
}
