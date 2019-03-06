using Nzh.Hero.Core.DbContext;
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

namespace Nzh.Hero.Service
{
    public class SysAreaService:BaseService, ISysAreaService
    {
        public SysAreaService(ISqlDbContext sqldb)
            : base(sqldb)
        {

        }

        public List<CityTreeDto> GetData(long pid)
        {
            var list = new List<CityTreeDto>();
            var query = Sqldb.Queryable<sys_citys>().Where(s => s.province_code == pid).ToList();
            list = query.Where(s => s.city_code == 0)
                .Select(s => new CityTreeDto() { name = s.name, pid = s.city_code, zipcode = s.zipcode }).ToList();
            var cunty = query.Where(s => s.city_level == 3)
                .Select(s => new CityTreeDto() { name = s.name, pid = s.city_code, zipcode = s.zipcode }).ToList();
            list.AddRange(cunty);
            return list;
        }

        public List<ZtreeDto> GetProvince()
        {
            var query = Sqldb.Queryable<sys_citys>().Where(s => s.province_code == 0)
                .Select(s => new ZtreeDto() { id = s.zipcode.ToString(), name = s.name, pId = "0" }).ToList();
            return query;
        }

        public bool IsExist(int areaCode, long id)
        {
            bool codeCount = false;
            if (id == 0)
            {
                var count = Sqldb.Queryable<sys_citys>().Where(s => s.zipcode == areaCode).Count();
                if (count > 0)
                {
                    codeCount = true;
                }
            }
            else
            {
                var count = Sqldb.Queryable<sys_citys>().Where(s => s.zipcode == areaCode && s.id != id).Count();
                if (count > 0)
                {
                    codeCount = true;
                }
            }
            return codeCount;
        }

        public sys_citys GetAreaById(string id)
        {
            return Sqldb.Queryable<sys_citys>().Where(s => s.id == SqlFunc.ToInt64(id)).First();
        }

        public void DelByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                Sqldb.Deleteable<sys_citys>().In(idsArray).ExecuteCommand();
            }
        }

        public List<CitySelDto> GetCitySel()
        {
            var data = new List<CitySelDto>();
            var list = Sqldb.Queryable<sys_citys>().OrderBy(s => s.zipcode).Select(s => new CitySelDto() { Id = s.id, Name = s.name, ParentId = s.province_code }).ToList();
            var fdata = list.Where(s => s.ParentId == 0).ToList();
            foreach (var item in fdata)
            {
                data.Add(item);
                var city = list.Where(s => s.ParentId == item.Id).ToList();
                if (city.Any())
                {
                    item.Children = city;
                    foreach (var citem in city)
                    {
                        InitChildList(list, citem, citem.Id);
                        list.Add(citem);
                    }
                }
            }
            return data;
        }

        public void InitChildList(List<CitySelDto> list, CitySelDto node, long pId)
        {

            var parentList = list.Where(s => s.ParentId == pId).ToList();
            if (parentList.Any())
            {
                foreach (var pitem in parentList)
                {
                    node.Children.Add(pitem);
                    InitChildList(list, pitem, pitem.Id);
                }
            }
        }

        public List<sys_citys> GetCountys(long pid)
        {
            if (pid == 0)
            {
                pid = 52;
            }
            return Sqldb.Queryable<sys_citys>().Where(s => s.province_code == pid && s.city_level == 3).ToList();
        }
    }
}
