using Microsoft.AspNetCore.Http;
using Nzh.Hero.Common.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Core.Web
{
    public class LoginCookieDto
    {
        public static string CookieScheme = "nzh_au_auth";
        public static string CookieClaim = "nzh_au_user";
    }
    public class CookieHelper
    {
        public static void WriteLoginCookie(string cliamsStr)
        {
            cliamsStr = Encrypt.AesEncrypt(cliamsStr);
            HttpContextExt.Current.Response.Cookies.Append(LoginCookieDto.CookieClaim, cliamsStr, new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddHours(10),
                Path = "/",
                HttpOnly = false,
                Secure = false
            });
        }

        public static bool ExistUserCookie()
        {

            var result = HttpContextExt.Current.Request.Cookies[LoginCookieDto.CookieClaim];
            if (result != null)
                return true;
            return false;
        }
        public static string GetUserLoginCookie()
        {
            var userCookie = HttpContextExt.Current.Request.Cookies[LoginCookieDto.CookieClaim];
            if (userCookie != null)
            {
                return Encrypt.AesDecrypt(userCookie);
            }
            return null;
        }

        public static void RemoveCooke()
        {
            HttpContextExt.Current.Response.Cookies.Delete(LoginCookieDto.CookieClaim);
        }
    }
}
