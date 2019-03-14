using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysFuncService
    {
        BootstrapGridDto GetData(BootstrapGridDto param);

        void InsertFunc(sys_operate dto);

        void UpdateFunc(sys_operate dto);

        sys_operate GetDataById(string id);

        void DelByIds(string ids);

    }
}
