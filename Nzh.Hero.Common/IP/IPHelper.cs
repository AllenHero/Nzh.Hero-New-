using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nzh.Hero.Common.IP
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public  class IPHelper
    {
        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}