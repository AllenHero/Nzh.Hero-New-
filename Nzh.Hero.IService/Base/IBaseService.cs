using Nzh.Hero.Core.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService.Base
{
    public interface IBaseService
    {
        LoginUserDto GetUserCookie();
    }       
}
