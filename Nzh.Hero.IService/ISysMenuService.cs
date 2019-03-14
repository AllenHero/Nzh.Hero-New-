using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Hero.IService
{
    public interface ISysMenuService 
    {
        List<sys_menu> GetMenuList(BootstrapGridDto param);

        List<sys_menu> GetMenuList();

        void AddMenu(sys_menu dto, string funcs);

        void UpdateMenu(sys_menu dto, string funcs);

        sys_menu GetMenuById(string id);

        List<long> GetMenuRefOpt(string id);

        List<sys_menu> GetTopMenuList();

        Task<List<RoleMenuDto>> GetRoleMenu();

        void DelByIds(string ids);

        List<sys_operate> GetFuncSelList();
    }
}
