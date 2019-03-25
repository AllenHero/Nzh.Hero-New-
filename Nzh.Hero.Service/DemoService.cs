using Nzh.Hero.Common.Security;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class DemoService : BaseService, IDemoService
    {
        public DemoService(ISqlDbContext sqldb)
           : base(sqldb)
        {

        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            var query = Sqldb.Queryable<demo>();
            int total = 0;
            var data = query.OrderBy(u => u.create_time, OrderByType.Desc)
                .Select(u => new { Id = u.id, Name = u.name, Sex = u.sex, Age = u.age,Remark=u.remark, CreateTime = u.create_time,CreatePerson=u.create_person })
                .ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }    

        public void InsertData(demo dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_time = DateTime.Now;
            dto.create_person = UserCookie.AccountName;
            dto.name = dto.name ?? string.Empty;
            dto.sex = dto.sex ?? string.Empty;                                                       
            dto.age = dto.age ;
            dto.remark = dto.remark ?? string.Empty;
            Sqldb.Insertable(dto).ExecuteCommand();
        }

        public void UpdateData(demo dto)
        {
            dto.name = dto.name ?? string.Empty;
            dto.sex = dto.sex ?? string.Empty;
            dto.age = dto.age;
            dto.remark = dto.remark ?? string.Empty;
            Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_person, s.create_time }).ExecuteCommand();
        }

        public void DelUserByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                Sqldb.Deleteable<demo>().In(idsArray).ExecuteCommand();
            }
        }

        public demo GetDemoByIds(string id)
        {
            return Sqldb.Queryable<demo>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
        }
    }
}
