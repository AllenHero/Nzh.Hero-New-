using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysRoleService 
    {
        BootstrapGridDto GetData(BootstrapGridDto param);

        void InsertRoleData(sys_role dto);

        void UpdateRoleData(sys_role dto);

        sys_role GetRoleById(string id);

        void DelRoleByIds(string ids);

        List<sys_menu> GetRoleAuthMenu(string roleid);

        List<ZtreeDto> GetRoleMenuTree(string roleid);

        void SaveRoleAuth(string roleId, string ids);

        List<sys_operate> GetOperateByRole(string menuId);

    }
}
