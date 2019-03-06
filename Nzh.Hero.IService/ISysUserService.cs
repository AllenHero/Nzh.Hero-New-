using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysUserService
    {
        sys_user LoginValidate(string uname, string pwd);

        void UpdateUserPwd(long id, string pwd);

        BootstrapGridDto GetData(BootstrapGridDto param, string accountName);

        List<sys_user> GetAllUser();

        bool CheckUserName(string uname, long id);

        void InsertData(sys_user dto);

        void UpdateData(sys_user dto);

        void UpdateProfile(sys_user dto);

        sys_user GetUserById(string id);

        void DelUserByIds(string ids);

        List<sys_role> GetRoleList();

    }
}
