using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Core.Web;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service.Base
{
    public class BaseService
    {
        protected readonly SqlSugarClient Sqldb;

        private readonly IsqlDbContext iSqlContext;

        protected LoginUserDto UserCookie;
        public BaseService(IsqlDbContext sqlContext)
        {
            UserCookie = GetUserCookie();
            iSqlContext = sqlContext;
            Sqldb = iSqlContext.DbInstance();
        }

        private LoginUserDto GetUserCookie()
        {
            var userClaims = CookieHelper.GetUserLoginCookie();
            if (userClaims != null)
                return userClaims.ToObject<LoginUserDto>();
            return new LoginUserDto();
        }
    }
}
