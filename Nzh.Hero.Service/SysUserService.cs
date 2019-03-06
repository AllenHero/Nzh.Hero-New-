using Nzh.Hero.Common.Security;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysUserService : BaseService
    {
        public SysUserService(IsqlDbContext sqldb)
           : base(sqldb)
        {

        }

        public sys_user LoginValidate(string uname, string pwd)
        {
            pwd = Encrypt.DesEncrypt(pwd);
            return Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname && s.pass_word == pwd).First();
        }

        public void UpdateUserPwd(long id, string pwd)
        {
            pwd = Encrypt.DesDecrypt(pwd);
            Sqldb.Updateable<sys_user>().UpdateColumns(s => new sys_user { pass_word = pwd }).Where(s => s.id == id).ExecuteCommand();
        }

        public BootstrapGridDto GetData(BootstrapGridDto param, string accountName)
        {
            var query = Sqldb.Queryable<sys_user, sys_role>((u, r) => new object[] { JoinType.Left, u.sys_role_id == r.id })
                           .Where((u, r) => !u.is_super);
            int total = 0;
            var data = query.OrderBy((u, r) => u.create_time, OrderByType.Desc)
                .Select((u, r) => new { Id = u.id, AccountName = u.account_name, RealName = u.real_name, MobilePhone = u.mobile_phone, Email = u.email, CreateTime = u.create_time, RoleName = r.role_name })
                .ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }

        public List<sys_user> GetAllUser()
        {
            return Sqldb.Queryable<sys_user>().OrderBy(s => s.id).ToList();
        }

        public bool CheckUserName(string uname, long id)
        {
            int count = 0;
            if (id == 0)
            {
                count = Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname.Trim()).Count();
            }
            else
            {
                count = Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname.Trim() && s.id != id).Count();
            }
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void InsertData(sys_user dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_time = DateTime.Now;
            dto.create_person = UserCookie.AccountName;
            dto.fax = dto.fax ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            Sqldb.Insertable(dto).ExecuteCommand();
        }

        public void UpdateData(sys_user dto)
        {
            dto.fax = dto.fax ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_person, s.create_time, s.account_name }).ExecuteCommand();
        }

        public void UpdateProfile(sys_user dto)
        {
            dto.fax = dto.fax ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            Sqldb.Updateable(dto).UpdateColumns(s => new { s.real_name, s.pass_word, s.mobile_phone, s.email }).ExecuteCommand();
        }

        public sys_user GetUserById(string id)
        {
            var data = Sqldb.Queryable<sys_user>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
            data.pass_word = Encrypt.DesDecrypt(data.pass_word.Trim());
            return data;
        }

        public void DelUserByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                Sqldb.Deleteable<sys_user>().In(idsArray).ExecuteCommand();
            }
        }

        public List<sys_role> GetRoleList()
        {
            return Sqldb.Queryable<sys_role>().ToList();
        }
    }
}
