using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nzh.Hero.Core.Web
{
    public class RequestHelper
    {
        public static string GetClientIp()
        {
            var ip = HttpContextExt.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContextExt.Current.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        public static string RequestGet(string key, string defVal)
        {
            if (HttpContextExt.Current.Request.Query.ContainsKey(key))
            {
                defVal = HttpContextExt.Current.Request.Query[key];
            }
            return defVal;
        }

        public static string RequestPost(string key, string defVal)
        {
            if (HttpContextExt.Current.Request.HasFormContentType && HttpContextExt.Current.Request.Form.ContainsKey(key))
            {
                defVal = HttpContextExt.Current.Request.Form[key];
            }
            return defVal;
        }

        public static string RequestEx(string key, string defVal)
        {
            string bodyText = string.Empty;
            using (var buffer = new StreamReader(HttpContextExt.Current.Request.Body))
            {
                bodyText = buffer.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(bodyText))
            {

            }
            return defVal;
        }

        public static string RequestBody()
        {
            string bodyText = string.Empty;
            var request = HttpContextExt.Current.Request;
            request.EnableRewind();
            using (var buffer = new StreamReader(request.Body))
            {
                bodyText = buffer.ReadToEnd();
            }
            return bodyText;
        }
    }
}
