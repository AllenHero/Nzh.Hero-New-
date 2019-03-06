using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysDicService :BaseService
    {
        public SysDicService(IDbContext sqldb)
           : base(sqldb)
        {

        }

        public List<sys_dictionary> GetData()
        {
            return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
        }

        public List<sys_dictionary> GetGridDataBypId(long pid)
        {
            return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == pid).ToList();
        }

        public void InsertDicData(sys_dictionary dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_person = UserCookie.AccountName;
            dto.create_time = DateTime.Now;
            if (dto.parent_id != 0)
            {
                var dicCode =
                    Sqldb.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();

                dto.dic_code = dicCode;
            }
            Sqldb.Insertable(dto).ExecuteCommand();
        }

        public void UpdateDicData(sys_dictionary dto)
        {
            if (dto.parent_id != 0)
            {
                var dicCode =
                    Sqldb.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();

                dto.dic_code = dicCode;
            }
            Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_person, s.create_time }).ExecuteCommand();
        }

        public sys_dictionary GetDicById(string id)
        {
            return Sqldb.Queryable<sys_dictionary>().InSingle(id);
        }

        public void DelByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                Sqldb.Deleteable<sys_dictionary>().In(idsArray).ExecuteCommand();
            }
        }

        public List<sys_dictionary> GetDicSelList()
        {
            return Sqldb.Queryable<sys_dictionary>().Where(s => s.parent_id == 0).ToList();
        }

        public List<ZtreeDto> GetDicZtree()
        {
            var data = Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == 0).Select(s => new ZtreeDto()
            {
                id = s.id.ToString(),
                name = s.dic_name,
                pId = s.parent_id.ToString()
            }).ToList();
            return data;
        }

        public List<sys_dictionary> GetTreeGrid()
        {
            return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
        }
    }
}
