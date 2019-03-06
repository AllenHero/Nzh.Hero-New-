using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysDicService
    {
        List<sys_dictionary> GetData();

        List<sys_dictionary> GetGridDataBypId(long pid);

        void InsertDicData(sys_dictionary dto);

        void UpdateDicData(sys_dictionary dto);

        sys_dictionary GetDicById(string id);

        void DelByIds(string ids);

        List<sys_dictionary> GetDicSelList();

        List<ZtreeDto> GetDicZtree();

        List<sys_dictionary> GetTreeGrid();
    }
}
