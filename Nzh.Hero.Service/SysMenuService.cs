using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Hero.Service
{
    public class SysMenuService:BaseService , ISysMenuService
    {
        private readonly ISysMenuRepository _sysmenuRepository;
        private readonly ISysMenuRefOperateRepository _sysmenurefoperateRepository;
        private readonly ISysFuncRepository _sysfuncRepository;
        private readonly ISysRoleAuthorizeRepository _sysroleauthorizeRepository;

        public SysMenuService(ISqlDbContext sqlContext, ISysMenuRepository sysmenuRepository, ISysMenuRefOperateRepository sysmenurefoperateRepository, ISysFuncRepository sysfuncRepository, ISysRoleAuthorizeRepository sysroleauthorizeRepository)
           : base(sqlContext)
        {
            _sysmenuRepository = sysmenuRepository;
            _sysmenurefoperateRepository = sysmenurefoperateRepository;
            _sysfuncRepository = sysfuncRepository;
            _sysroleauthorizeRepository = sysroleauthorizeRepository;
        }

        public List<sys_menu> GetMenuList(BootstrapGridDto param)
        {
            //var query = Sqldb.Queryable<sys_menu>().OrderBy(s => s.menu_sort).ToList();
            var query = _sysmenuRepository.Queryable<sys_menu>().OrderBy(s => s.menu_sort).ToList();
            return query;
        }

        public List<sys_menu> GetMenuList()
        {
            //var query = Sqldb.Queryable<sys_menu>().OrderBy(s => s.menu_sort) .ToList();
            var query = _sysmenuRepository.Queryable<sys_menu>().OrderBy(s => s.menu_sort).ToList();
            return query;
        }

        public void AddMenu(sys_menu dto, string funcs)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_time = DateTime.Now;
            dto.create_person = "admin";
            if (!string.IsNullOrEmpty(dto.menu_url))
            {
                dto.menu_type = 1;
            }
            if (dto.parent_id == 0)
            {
                dto.menu_level = 1;
            }
            else
            {
                //dto.menu_level = Sqldb.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
                dto.menu_level = _sysmenuRepository.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
            }
            //Sqldb.Insertable(dto).ExecuteCommand();
            _sysmenuRepository.Insert(dto);
            if (!string.IsNullOrEmpty(funcs))
            {
                var funcArray = funcs.Split(',');
                if (funcArray.Length > 0)
                {
                    var list = new List<sys_menu_ref_operate>();
                    foreach (var func in funcArray)
                    {
                        var funcModel = new sys_menu_ref_operate();
                        funcModel.menu_id = dto.id;
                        funcModel.operate_id = func.ToInt64();
                        list.Add(funcModel);
                    }
                    //Sqldb.Insertable(list).ExecuteCommand();
                    _sysmenurefoperateRepository.InsertRange(list);
                }
            }
        }

        public void UpdateMenu(sys_menu dto, string funcs)
        {
            sys_menu sys_menu = _sysmenuRepository.GetById(dto.id);
            if (!string.IsNullOrEmpty(dto.menu_url))
            {
                dto.menu_type = 1;
            }
            if (dto.parent_id == 0)
            {
                dto.menu_level = 1;
            }
            else
            {
                //dto.menu_level = Sqldb.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
                dto.menu_level = _sysmenuRepository.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
            }
            dto.create_person = sys_menu.create_person ?? string.Empty;
            dto.create_time = sys_menu.create_time ;
            //Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_time, s.create_person }).ExecuteCommand();
            _sysmenuRepository.Update(dto);
            //Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => s.menu_id == dto.id).ExecuteCommand();
            _sysmenurefoperateRepository.Delete(s => s.menu_id == dto.id);
            if (!string.IsNullOrEmpty(funcs))
            {
                var funcArray = funcs.Split(',');
                if (funcArray.Length > 0)
                {
                    var list = new List<sys_menu_ref_operate>();
                    foreach (var func in funcArray)
                    {
                        var funcModel = new sys_menu_ref_operate();
                        funcModel.menu_id = dto.id;
                        funcModel.operate_id = func.ToInt64();
                        list.Add(funcModel);
                    }
                    //Sqldb.Insertable(list).ExecuteCommand();
                    _sysmenurefoperateRepository.InsertRange(list);
                }
            }
        }

        public sys_menu GetMenuById(string id)
        {
            //return Sqldb.Queryable<sys_menu>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
            return _sysmenuRepository.GetById(id);
        }

        public List<long> GetMenuRefOpt(string id)
        {
            //return Sqldb.Queryable<sys_menu_ref_operate>().Where(s => s.menu_id == SqlFunc.ToInt64(id)).Select(s => s.operate_id).ToList();
            return _sysmenurefoperateRepository.Queryable<sys_menu_ref_operate>().Where(s => s.menu_id == SqlFunc.ToInt64(id)).Select(s => s.operate_id).ToList();
        }

        public List<sys_menu> GetTopMenuList()
        {
            var list = new List<sys_menu>();
            if (UserCookie.IsSuper)
            {
                //list = Sqldb.Queryable<sys_menu>().Where(s => s.parent_id == 0).OrderBy(s => s.menu_sort).ToList();
                list = _sysmenuRepository.Queryable<sys_menu>().Where(s => s.parent_id == 0).OrderBy(s => s.menu_sort).ToList();
            }
            else
            {
                //list = Sqldb.Queryable<sys_menu, sys_role_authorize>((m, r) => new object[] { JoinType.Inner, m.id == r.menu_id }) .Where((m, r) => r.role_id == UserCookie.SysRoleId && m.parent_id == 0) .OrderBy((m, r) => m.menu_sort) .Select((m, r) => m).ToList();
                list = Sqldb.Queryable<sys_menu, sys_role_authorize>((m, r) => new object[] { JoinType.Inner, m.id == r.menu_id }).Where((m, r) => r.role_id == UserCookie.SysRoleId && m.parent_id == 0).OrderBy((m, r) => m.menu_sort).Select((m, r) => m).ToList();
            }
            return list;
        }

        public async Task<List<RoleMenuDto>> GetRoleMenu()
        {
            var list = new List<RoleMenuDto>();
            if (UserCookie.IsSuper)
            {
                //list = await Sqldb.Queryable<sys_menu>().OrderBy(s => s.menu_sort).Select(s => new RoleMenuDto(){id = s.id, menu_name = s.menu_name,menu_sort = s.menu_sort,menu_url = s.menu_url, parent_id = s.parent_id,menu_type = s.menu_type,menu_icon = s.menu_icon }).ToListAsync();
                list = await _sysmenuRepository.Queryable<sys_menu>().OrderBy(s => s.menu_sort).Select(s => new RoleMenuDto() { id = s.id, menu_name = s.menu_name, menu_sort = s.menu_sort, menu_url = s.menu_url, parent_id = s.parent_id, menu_type = s.menu_type, menu_icon = s.menu_icon }).ToListAsync();
            }
            else
            {
                //list =await Sqldb.Queryable<sys_menu, sys_role_authorize>((m, r) => new object[] { JoinType.Inner, m.id == r.menu_id }).Where((m, r) => r.role_id == UserCookie.SysRoleId) .OrderBy((m, r) => m.menu_sort).Select((m, r) => new RoleMenuDto(){id = m.id, menu_name = m.menu_name, menu_sort = m.menu_sort,menu_url = m.menu_url,parent_id = m.parent_id,menu_type = m.menu_type,menu_icon = m.menu_icon}).ToListAsync();
                list = await Sqldb.Queryable<sys_menu, sys_role_authorize>((m, r) => new object[] { JoinType.Inner, m.id == r.menu_id }).Where((m, r) => r.role_id == UserCookie.SysRoleId).OrderBy((m, r) => m.menu_sort).Select((m, r) => new RoleMenuDto() { id = m.id, menu_name = m.menu_name, menu_sort = m.menu_sort, menu_url = m.menu_url, parent_id = m.parent_id, menu_type = m.menu_type, menu_icon = m.menu_icon }).ToListAsync();
            }
            return list;
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
                    _sysmenuRepository.BeginTran();
                    //Sqldb.Deleteable<sys_menu>().In(idsArray).ExecuteCommand();
                    _sysmenuRepository.DeleteById(idsArray);
                    //Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => arri.Contains(s.menu_id)).ExecuteCommand();
                    _sysmenurefoperateRepository.Delete(s => arri.Contains(s.menu_id));
                    //Sqldb.Deleteable<sys_role_authorize>().Where(s => arri.Contains(s.menu_id) || arri.Contains(s.menu_pid)).ExecuteCommand();
                    _sysroleauthorizeRepository.Delete(s => arri.Contains(s.menu_id) || arri.Contains(s.menu_pid));
                    //Sqldb.Ado.CommitTran();
                    _sysmenuRepository.CommitTran();
                }
            }
            catch (Exception ex)
            {
                //Sqldb.Ado.RollbackTran();
                _sysmenuRepository.RollbackTran();
                throw ex;
            }
        }

        public List<sys_operate> GetFuncSelList()
        {
            //return Sqldb.Queryable<sys_operate>().OrderBy(s => s.func_sort).ToList();
            return _sysfuncRepository.Queryable<sys_operate>().OrderBy(s => s.func_sort).ToList();
        }
    }
}
