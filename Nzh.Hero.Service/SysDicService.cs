using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Service
{
    public class SysDicService :BaseService , ISysDicService
    {
        private readonly ISysDicRepository _sysdicRepository;

        public SysDicService(ISqlDbContext sqldb, ISysDicRepository sysdicRepository): base(sqldb)
        {
            _sysdicRepository = sysdicRepository;
        }

        public List<sys_dictionary> GetData()
        {
            //return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
            return _sysdicRepository.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
        }

        public List<sys_dictionary> GetGridDataBypId(long pid)
        {
            //return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == pid).ToList();
            return _sysdicRepository.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == pid).ToList();
        }

        public void InsertDicData(sys_dictionary dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_person = UserCookie.AccountName;
            dto.create_time = DateTime.Now;
            if (dto.parent_id != 0)
            {
                //var dicCode =Sqldb.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();
                var dicCode = _sysdicRepository.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();
                dto.dic_code = dicCode;
            }
            //Sqldb.Insertable(dto).ExecuteCommand();
            _sysdicRepository.Insert(dto);
        }

        public void UpdateDicData(sys_dictionary dto)
        {
            sys_dictionary sys_dictionary = _sysdicRepository.GetById(dto.id);
            if (dto.parent_id != 0)
            {
                //var dicCode =Sqldb.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();
                var dicCode = _sysdicRepository.Queryable<sys_dictionary>().Where(s => s.id == dto.parent_id).Select(s => s.dic_code).First();
                dto.dic_code = dicCode;
            }
            dto.create_person = sys_dictionary.create_person ?? string.Empty;
            dto.create_time = sys_dictionary.create_time;
            //Sqldb.Updateable(dto).IgnoreColumns(s => new { s.create_person, s.create_time }).ExecuteCommand();
            _sysdicRepository.Update(dto);
        }

        public sys_dictionary GetDicById(string id)
        {
            //return Sqldb.Queryable<sys_dictionary>().InSingle(id);
            return _sysdicRepository.GetById(id);
        }

        public void DelByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                //Sqldb.Deleteable<sys_dictionary>().In(idsArray).ExecuteCommand();
                _sysdicRepository.DeleteById(idsArray);
            }
        }

        public List<sys_dictionary> GetDicSelList()
        {
            //return Sqldb.Queryable<sys_dictionary>().Where(s => s.parent_id == 0).ToList();
            return _sysdicRepository.Queryable<sys_dictionary>().Where(s => s.parent_id == 0).ToList();
        }

        public List<ZtreeDto> GetDicZtree()
        {
            //var data = Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == 0).Select(s => new ZtreeDto(){id = s.id.ToString(),name = s.dic_name,pId = s.parent_id.ToString()}).ToList();
            var data = _sysdicRepository.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).Where(s => s.parent_id == 0).Select(s => new ZtreeDto() { id = s.id.ToString(), name = s.dic_name, pId = s.parent_id.ToString() }).ToList();
            return data;
        }

        public List<sys_dictionary> GetTreeGrid()
        {
            //return Sqldb.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
            return _sysdicRepository.Queryable<sys_dictionary>().OrderBy(s => s.sort_num).ToList();
        }
    }
}
