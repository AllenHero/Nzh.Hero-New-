using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface IDemoService 
    {
        BootstrapGridDto GetData(BootstrapGridDto param);

        void InsertData(demo dto);

        void UpdateData(demo dto);

        void DelUserByIds(string ids);

        demo GetTestById(string id);

    }
}
