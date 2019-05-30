using Nzh.Hero.Common.IP;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysLogService : BaseService, ISysLogService
    {
        private readonly ISysLogRepository _syslogRepository;

        public SysLogService(ISqlDbContext sqldb, ISysLogRepository syslogRepository)
           : base(sqldb)
        {
            _syslogRepository = syslogRepository;
        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            //var query = Sqldb.Queryable<sys_log>();
            var query = _syslogRepository.Queryable<sys_log>();
            int total = 0;
            var data = query.OrderBy(u => u.logtime, OrderByType.Desc).Select(u => new { Id = u.id, Operation = u.operation, LogTime = u.logtime, LogtType = u.logtype, LogMsg = u.logmsg, LogLevel = u.loglevel, LogIP = u.logip }).ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }

        public void WriteLog(LogType logtype,string logmsg, LogState logstate)
        {
            sys_log log = new sys_log();
            log.id = IdWorkerHelper.NewId();
            log.operation = UserCookie.AccountName;
            log.logtime = DateTime.Now;
            log.logtype = logtype.ToString();
            log.logmsg = logmsg;
            log.loglevel = logstate.ToString();
            log.logip = "127.0.0.1";  //TODO
            //Sqldb.Insertable(log).ExecuteCommand();
            _syslogRepository.Insert(log);
        }
    }
}
