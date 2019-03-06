using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysFuncService :BaseService
    {
        public SysFuncService(IsqlDbContext sqldb)
       : base(sqldb)
        {

        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            int total = 0;
            var data = Sqldb.Queryable<sys_operate>()
                .OrderBy(s => s.func_sort)
                .ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }

        public void InsertFunc(sys_operate dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.func_icon = dto.func_icon ?? "tag";
            dto.func_class = dto.func_class ?? "btn-blue";
            dto.create_time = DateTime.Now;
            dto.create_person = UserCookie.AccountName;
            Sqldb.Insertable(dto).ExecuteCommand();
        }

        public void UpdateFunc(sys_operate dto)
        {
            dto.func_icon = dto.func_icon ?? "tag";
            dto.func_class = dto.func_class ?? "btn-blue";
            Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_time, s.create_person }).ExecuteCommand();
        }

        public sys_operate GetDataById(string id)
        {
            return Sqldb.Queryable<sys_operate>().InSingle(id);
        }

        public void DelByIds(string ids)
        {
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    var idsArray = ids.Split(',');
                    long[] arri = idsArray.StrToLongArray();

                    Sqldb.Ado.BeginTran();
                    Sqldb.Deleteable<sys_operate>().In(idsArray).ExecuteCommand();
                    Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => arri.Contains(s.operate_id)).ExecuteCommand();
                    Sqldb.Deleteable<sys_role_authorize>()
                        .Where(s => arri.Contains(s.menu_id)).ExecuteCommand();
                    Sqldb.Ado.CommitTran();
                }
            }
            catch (Exception ex)
            {
                Sqldb.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}
