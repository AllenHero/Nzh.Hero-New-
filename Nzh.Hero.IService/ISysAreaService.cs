using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysAreaService 
    {
        List<CityTreeDto> GetData(long pid);

        List<ZtreeDto> GetProvince();

        bool IsExist(int areaCode, long id);

        sys_citys GetAreaById(string id);

        void DelByIds(string ids);

        List<CitySelDto> GetCitySel();

        void InitChildList(List<CitySelDto> list, CitySelDto node, long pId);

        List<sys_citys> GetCountys(long pid);

        void InsertAreaData(sys_citys dto);

        void UpdateAreaData(sys_citys dto);
    }
}
