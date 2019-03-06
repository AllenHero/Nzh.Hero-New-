using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
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
    public class SysMenuService:BaseService
    {
        public SysMenuService(ISqlDbContext sqlContext)
           : base(sqlContext)
        {

        }

        public List<sys_menu> GetMenuList(BootstrapGridDto param)
        {
            //int total = 0;
            var query = Sqldb.Queryable<sys_menu>().OrderBy(s => s.menu_sort).ToList();
            return query;
        }

        public List<sys_menu> GetMenuList()
        {
            var query = Sqldb.Queryable<sys_menu>()
               .OrderBy(s => s.menu_sort)
               .ToList();
            return query;
        }

        public void AddMenu(sys_menu dto, string funcs)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_time = DateTime.Now;
            dto.create_person = "admin";
            //dto.menu_icon = dto.menu_font;
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
                dto.menu_level = Sqldb.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
            }
            Sqldb.Insertable(dto).ExecuteCommand();
            //设置菜单下按钮
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
                    Sqldb.Insertable(list).ExecuteCommand();
                }
            }
        }

        public void UpdateMenu(sys_menu dto, string funcs)
        {
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
                dto.menu_level = Sqldb.Queryable<sys_menu>().Where(s => s.id == dto.parent_id).Select(s => s.menu_level).First() + 1;
            }
            Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_time, s.create_person }).ExecuteCommand();
            //设置菜单下按钮
            Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => s.menu_id == dto.id).ExecuteCommand();
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
                    Sqldb.Insertable(list).ExecuteCommand();
                }

            }
        }

        public sys_menu GetMenuById(string id)
        {
            return Sqldb.Queryable<sys_menu>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
        }

        public List<long> GetMenuRefOpt(string id)
        {
            return
                Sqldb.Queryable<sys_menu_ref_operate>()
                    .Where(s => s.menu_id == SqlFunc.ToInt64(id))
                    .Select(s => s.operate_id)
                    .ToList();
        }

        public List<sys_menu> GetTopMenuList()
        {
            var list = new List<sys_menu>();
            if (UserCookie.IsSuper)
            {
                list =
                    Sqldb.Queryable<sys_menu>()
                        .Where(s => s.parent_id == 0)
                        .OrderBy(s => s.menu_sort)
                        .ToList();
            }
            else
            {
                list =
                    Sqldb.Queryable<sys_menu, sys_role_authorize>(
                        (m, r) => new object[] { JoinType.Inner, m.id == r.menu_id })
                        .Where((m, r) => r.role_id == UserCookie.SysRoleId && m.parent_id == 0)
                        .OrderBy((m, r) => m.menu_sort)
                        .Select((m, r) => m)
                        .ToList();
            }
            return list;
        }

        public async Task<List<RoleMenuDto>> GetRoleMenu()
        {
            var list = new List<RoleMenuDto>();
            if (UserCookie.IsSuper)
            {
                list = await Sqldb.Queryable<sys_menu>().OrderBy(s => s.menu_sort).Select(s => new RoleMenuDto()
                {
                    id = s.id,
                    menu_name = s.menu_name,
                    menu_sort = s.menu_sort,
                    menu_url = s.menu_url,
                    parent_id = s.parent_id,
                    menu_type = s.menu_type,
                    menu_icon = s.menu_icon
                }).ToListAsync();

            }
            else
            {
                list =
                    await Sqldb.Queryable<sys_menu, sys_role_authorize>(
                       (m, r) => new object[] { JoinType.Inner, m.id == r.menu_id })
                       .Where((m, r) => r.role_id == UserCookie.SysRoleId)
                       .OrderBy((m, r) => m.menu_sort)
                       .Select((m, r) => new RoleMenuDto()
                       {
                           id = m.id,
                           menu_name = m.menu_name,
                           menu_sort = m.menu_sort,
                           menu_url = m.menu_url,
                           parent_id = m.parent_id,
                           menu_type = m.menu_type,
                           menu_icon = m.menu_icon
                       }).ToListAsync();
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
                    Sqldb.Ado.BeginTran();
                    Sqldb.Deleteable<sys_menu>().In(idsArray).ExecuteCommand();
                    Sqldb.Deleteable<sys_menu_ref_operate>().Where(s => arri.Contains(s.menu_id)).ExecuteCommand();
                    Sqldb.Deleteable<sys_role_authorize>()
                        .Where(s => arri.Contains(s.menu_id) || arri.Contains(s.menu_pid))
                        .ExecuteCommand();
                    Sqldb.Ado.CommitTran();
                }
            }
            catch (Exception ex)
            {
                Sqldb.Ado.RollbackTran();
                throw ex;
            }
        }

        public List<sys_operate> GetFuncSelList()
        {
            return Sqldb.Queryable<sys_operate>().OrderBy(s => s.func_sort).ToList();
        }
    }
}
