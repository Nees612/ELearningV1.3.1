using Microsoft.AspNetCore.Http;
using System;


namespace ELearningV1._3._1.Managers
{
    public static class CookieOptionsManager
    {
        public static CookieOptions CreateCookieOption(int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            //option.Domain = "http://localhost:5000/";
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            return option;
        }

    }
}
