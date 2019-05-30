using Nzh.Hero.Common.Security;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
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
    public class SysUserService : BaseService, ISysUserService
    {
        private readonly ISysUserRepository _sysuserRepository;
        private readonly ISysRoleRepository _sysroleRepository;
       

        public SysUserService(ISqlDbContext sqldb, ISysUserRepository sysuserRepository, ISysRoleRepository sysroleRepository): base(sqldb)
        {
            _sysuserRepository = sysuserRepository;
            _sysroleRepository = sysroleRepository;
        }

        public sys_user LoginValidate(string uname, string pwd)
        {
            pwd = Encrypt.DesEncrypt(pwd);
            //return Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname && s.pass_word == pwd).First();
            return _sysuserRepository.Queryable<sys_user>().Where(s => s.account_name == uname && s.pass_word == pwd).First();
        }

        public void UpdateUserPwd(long id, string pwd)
        {
            pwd = Encrypt.DesDecrypt(pwd);
            //Sqldb.Updateable<sys_user>().UpdateColumns(s => new sys_user { pass_word = pwd }).Where(s => s.id == id).ExecuteCommand();
            sys_user dto = _sysuserRepository.GetById(id);
            dto.pass_word = pwd ?? string.Empty;
            dto.account_name = dto.account_name ?? string.Empty;
            dto.real_name = dto.real_name ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.fax = dto.fax ?? string.Empty;
            dto.create_person = dto.create_person ?? string.Empty;
            dto.create_time = dto.create_time ;
            dto.is_super = dto.is_super;
            dto.sys_role_id = dto.sys_role_id;
            dto.province = dto.province;
            dto.city = dto.city;
            dto.county = dto.county;
            dto.user_level = dto.user_level;
            dto.remark = dto.remark ??string.Empty;
            _sysuserRepository.Update(dto);
        }

        public BootstrapGridDto GetData(BootstrapGridDto param, string accountName)
        {
            //var query = Sqldb.Queryable<sys_user, sys_role>((u, r) => new object[] { JoinType.Left, u.sys_role_id == r.id }).Where((u, r) => !u.is_super);
            var query = Sqldb.Queryable<sys_user, sys_role>((u, r) => new object[] { JoinType.Left, u.sys_role_id == r.id }).Where((u, r) => !u.is_super);
            int total = 0;
            var data = query.OrderBy((u, r) => u.create_time, OrderByType.Desc) .Select((u, r) => new { Id = u.id, AccountName = u.account_name, RealName = u.real_name, MobilePhone = u.mobile_phone, Email = u.email, CreateTime = u.create_time, RoleName = r.role_name }).ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }

        public List<sys_user> GetAllUser()
        {
            //return Sqldb.Queryable<sys_user>().OrderBy(s => s.id).ToList();
            return _sysuserRepository.Queryable<sys_user>().OrderBy(s => s.id).ToList();
        }

        public bool CheckUserName(string uname, long id)
        {
            int count = 0;
            if (id == 0)
            {
                //count = Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname.Trim()).Count();
                count = _sysuserRepository.Queryable<sys_user>().Where(s => s.account_name == uname.Trim()).Count();
            }
            else
            {
                //count = Sqldb.Queryable<sys_user>().Where(s => s.account_name == uname.Trim() && s.id != id).Count();
                count = _sysuserRepository.Queryable<sys_user>().Where(s => s.account_name == uname.Trim() && s.id != id).Count();
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
            //Sqldb.Insertable(dto).ExecuteCommand();
            _sysuserRepository.Insert(dto);
        }

        public void UpdateData(sys_user dto)
        {
            sys_user sys_user = _sysuserRepository.GetById(dto.id);
            dto.fax = dto.fax ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            dto.create_person = sys_user.create_person ?? string.Empty;
            dto.create_time = sys_user.create_time;
            dto.account_name = sys_user.account_name ??string.Empty;
            //Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_person, s.create_time, s.account_name }).ExecuteCommand();
            _sysuserRepository.Update(dto);
        }

        public void UpdateProfile(sys_user dto)
        {
            sys_user sys_user = _sysuserRepository.GetById(dto.id);
            dto.fax = dto.fax ?? string.Empty;
            dto.email = dto.email ?? string.Empty;
            dto.mobile_phone = dto.mobile_phone ?? string.Empty;
            dto.real_name = sys_user.real_name ?? string.Empty;
            dto.pass_word = sys_user.pass_word ?? string.Empty;
            dto.mobile_phone = sys_user.mobile_phone ?? string.Empty;
            dto.email = sys_user.email ?? string.Empty;
            //Sqldb.Updateable(dto).UpdateColumns(s => new { s.real_name, s.pass_word, s.mobile_phone, s.email }).ExecuteCommand();
            _sysuserRepository.Update(dto);
        }

        public sys_user GetUserById(string id)
        {
            //var data = Sqldb.Queryable<sys_user>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
            var data = _sysuserRepository.Queryable<sys_user>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
            data.pass_word = Encrypt.DesDecrypt(data.pass_word.Trim());
            return data;
        }

        public void DelUserByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                //Sqldb.Deleteable<sys_user>().In(idsArray).ExecuteCommand();
                _sysuserRepository.DeleteById(idsArray);
            }
        }

        public List<sys_role> GetRoleList()
        {
            //return Sqldb.Queryable<sys_role>().ToList();
            return _sysroleRepository.Queryable<sys_role>().ToList();
        }
    }
}
