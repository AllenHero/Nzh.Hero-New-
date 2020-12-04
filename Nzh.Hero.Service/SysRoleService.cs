using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.JsonExt;
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
    public class SysRoleService : BaseService, ISysRoleService
    {
        private readonly ISysRoleRepository _sysroleRepository;
        private readonly ISysRoleAuthorizeRepository _sysroleauthorizeRepository;
        private readonly ISysMenuRepository _sysmenuRepository;

        public SysRoleService(ISqlDbContext sqldb, ISysRoleRepository sysroleRepository, ISysRoleAuthorizeRepository sysroleauthorizeRepository, ISysMenuRepository sysmenuRepository): base(sqldb)
        {
            _sysroleRepository = sysroleRepository;
            _sysroleauthorizeRepository = sysroleauthorizeRepository;
            _sysmenuRepository = sysmenuRepository;
        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            int total = 0;
            var query = _sysroleRepository.Queryable<sys_role>().OrderBy(s => s.sort).ToPageList(param.page, param.limit, ref total);
            param.total = total;
            param.rows = query;
            return param;
        }

        public void InsertRoleData(sys_role dto)
        {
            dto.role_code = dto.role_code ?? string.Empty;
            dto.id = IdWorkerHelper.NewId();
            dto.create_person = UserCookie.AccountName;
            dto.create_time = DateTime.Now;
            _sysroleRepository.Insert(dto);
        }

        public void UpdateRoleData(sys_role dto)
        {
            sys_role sys_role = _sysroleRepository.GetById(dto.id);
            dto.role_code = dto.role_code ?? string.Empty;
            dto.create_person = sys_role.create_person ?? string.Empty;
            dto.create_time = sys_role.create_time;
            _sysroleRepository.Update(dto);
        }

        public sys_role GetRoleById(string id)
        {
            return _sysroleRepository.GetById(id);
        }

        public void DelRoleByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',').StrToLongArray();
                _sysroleRepository.DeleteById(idsArray);
                _sysroleauthorizeRepository.Delete(s => idsArray.Contains(s.role_id));
            }
        }

        public List<sys_menu> GetRoleAuthMenu(string roleid)
        {
            var data = Sqldb.Queryable<sys_menu, sys_role_authorize>((m, r) => m.id == r.menu_id).Where((m, r) => r.role_id == SqlFunc.ToInt64(roleid)).Select((m, r) => m).ToList();
            return data;
        }

        public List<ZtreeDto> GetRoleMenuTree(string roleid)
        {
            var menu = _sysmenuRepository.Queryable<sys_menu>().OrderBy(s => s.menu_sort).Select(s => new ZtreeDto() { id = s.id.ToString(), name = s.menu_name, pId = s.parent_id.ToString() }).ToList();
            var func = Sqldb.Queryable<sys_menu_ref_operate, sys_operate, sys_menu>((f, o, m) => f.operate_id == o.id && f.menu_id == m.id).Select((f, o, m) => new ZtreeDto() { id = f.operate_id.ToString(), pId = f.menu_id.ToString(), name = o.func_cname }).ToList();
            menu.AddRange(func);
            var role = _sysroleauthorizeRepository.Queryable<sys_role_authorize>().Where(s => s.role_id == SqlFunc.ToInt64(roleid)).Select(s => new { s.menu_id, s.menu_pid }).ToList();
            if (role.Any())
            {
                foreach (var item in menu)
                {
                    var isok = role.Where(s => s.menu_id == item.id.ToInt64() && s.menu_pid == item.pId.ToInt64()).Count();
                    if (isok > 0)
                    {
                        item.checkstate = true;
                    }
                }
            }
            return menu;
        }

        public void SaveRoleAuth(string roleId, string ids)
        {
            _sysroleauthorizeRepository.Delete(s => s.role_id == SqlFunc.ToInt64(roleId));
            if (!string.IsNullOrEmpty(ids))
            {
                var list = new List<sys_role_authorize>();
                var menuIds = ids.ToObject<List<ZtreeDto>>();
                foreach (var mid in menuIds)
                {
                    mid.pId = mid.pId ?? "0";
                    var model = new sys_role_authorize();
                    model.id = IdWorkerHelper.NewId();
                    model.role_id = roleId.ToInt64();
                    model.menu_id = mid.id.ToInt64();
                    model.menu_pid = mid.pId.ToInt64();
                    model.create_time = DateTime.Now;
                    model.create_person = UserCookie.AccountName;
                    list.Add(model);
                }
                if (list.Any())
                {
                    _sysroleauthorizeRepository.InsertRange(list);
                }
            }
        }

        public List<sys_operate> GetOperateByRole(string menuId)
        {
            var list = new List<sys_operate>();
            if (UserCookie.IsSuper)
            {
                list = Sqldb.Queryable<sys_operate, sys_menu_ref_operate>((f, m) => f.id == m.operate_id && m.menu_id == SqlFunc.ToInt64(menuId)).Select((f, m) => f).ToList();
            }
            else
            {
                list = Sqldb.Queryable<sys_operate, sys_role_authorize>((f, r) => f.id == r.menu_id).Where((f, r) => r.role_id == UserCookie.SysRoleId && r.menu_pid == SqlFunc.ToInt64(menuId)).Select((f, m) => f).ToList();
            }
            return list;
        }
    }
}
