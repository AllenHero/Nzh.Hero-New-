using Nzh.Hero.Common.Extends;
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
using System.Linq;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysFuncService :BaseService , ISysFuncService
    {
        private readonly ISysFuncRepository _sysfuncRepository;
        private readonly ISysMenuRefOperateRepository _sysmenurefoperateRepository;
        private readonly ISysRoleAuthorizeRepository _sysroleauthorizeRepository;

        public SysFuncService(ISqlDbContext sqldb, ISysFuncRepository sysfuncRepository,ISysMenuRefOperateRepository sysmenurefoperateRepository, ISysRoleAuthorizeRepository sysroleauthorizeRepository)
       : base(sqldb)
        {
            _sysfuncRepository = sysfuncRepository;
            _sysmenurefoperateRepository = sysmenurefoperateRepository;
            _sysroleauthorizeRepository = sysroleauthorizeRepository;
        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            int total = 0;
            //var data = Sqldb.Queryable<sys_operate>().OrderBy(s => s.create_time, OrderByType.Desc).Select(u => new { Id = u.id, FuncName = u.func_name, FuncCName = u.func_cname, FuncIcon = u.func_icon, FuncSort = u.func_sort, FuncClass=u.func_class, CreateTime = u.create_time, CreatePerson = u.create_person }) .ToPageList(param.page, param.limit, ref total);
            var data = _sysfuncRepository.Queryable<sys_operate>().OrderBy(s => s.create_time, OrderByType.Desc).Select(u => new { Id = u.id, FuncName = u.func_name, FuncCName = u.func_cname, FuncIcon = u.func_icon, FuncSort = u.func_sort, FuncClass = u.func_class, CreateTime = u.create_time, CreatePerson = u.create_person }).ToPageList(param.page, param.limit, ref total);
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
            //Sqldb.Insertable(dto).ExecuteCommand();
            _sysfuncRepository.Insert(dto);
        }

        public void UpdateFunc(sys_operate dto)
        {
            sys_operate sys_operate = _sysfuncRepository.GetById(dto.id);
            dto.func_icon = dto.func_icon ?? "tag";
            dto.func_class = dto.func_class ?? "btn-blue";
            dto.create_person = sys_operate.create_person ?? string.Empty;
            dto.create_time = sys_operate.create_time;
            //Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_time, s.create_person }).ExecuteCommand();
            _sysfuncRepository.Update(dto);
        }

        public sys_operate GetFuncByIds(string id)
        {
            //return Sqldb.Queryable<sys_operate>().InSingle(id);
            return _sysfuncRepository.GetById(id);
        }

        public void DelByIds(string ids)
        {
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    var idsArray = ids.Split(',');
                    long[] arri = idsArray.StrToLongArray();
                    //Sqldb.Ado.BeginTran();
                    _sysfuncRepository.BeginTran();
                    //Sqldb.Deleteable<sys_operate>().In(idsArray).ExecuteCommand();
                    _sysfuncRepository.DeleteById(idsArray);
                    //Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => arri.Contains(s.operate_id)).ExecuteCommand();
                    _sysmenurefoperateRepository.Delete(s => arri.Contains(s.operate_id));
                    //Sqldb.Deleteable<sys_role_authorize>() .Where(s => arri.Contains(s.menu_id)).ExecuteCommand();
                    _sysroleauthorizeRepository.Delete(s => arri.Contains(s.menu_id));
                    //Sqldb.Ado.CommitTran();
                    _sysfuncRepository.CommitTran();
                }
            }
            catch (Exception ex)
            {
                //Sqldb.Ado.RollbackTran();
                _sysfuncRepository.RollbackTran();
                throw ex;
            }
        }
    }
}
