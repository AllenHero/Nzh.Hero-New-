using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class LogService : BaseService, ILogService
    {
        public LogService(ISqlDbContext sqldb)
           : base(sqldb)
        {

        }

        public void WriteLog(sys_log dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.operation = UserCookie.AccountName;
            dto.logtime = DateTime.Now;
            dto.logtype = dto.logtype;
            dto.logmsg = dto.logmsg;
            dto.loglevel = dto.loglevel;
            dto.logip = dto.logip;
            Sqldb.Insertable(dto).ExecuteCommand();
        }
    }
}
