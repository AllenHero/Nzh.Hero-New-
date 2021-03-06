﻿using Nzh.Hero.Common.Security;
using Nzh.Hero.Common.Snowflake;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service.Base;
using Nzh.Hero.ViewModel.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nzh.Hero.Service
{
    public class DemoService : BaseService, IDemoService
    {
        private readonly IDemoRepository _demoRepository;

        public DemoService(ISqlDbContext sqldb,IDemoRepository demoRepository): base(sqldb)
        {
            _demoRepository = demoRepository;
        }

        public BootstrapGridDto GetData(BootstrapGridDto param)
        {
            var query = _demoRepository.Queryable<demo>();
            int total = 0;
            var data = query.OrderBy(u => u.create_time, OrderByType.Desc).Select(u => new { Id = u.id, Name = u.name, Sex = u.sex, Age = u.age, Remark = u.remark, CreateTime = u.create_time, CreatePerson = u.create_person }).ToPageList(param.page, param.limit, ref total);
            param.rows = data;
            param.total = total;
            return param;
        }    

        public void InsertData(demo dto)
        {
            dto.id = IdWorkerHelper.NewId();
            dto.create_time = DateTime.Now;
            dto.create_person = UserCookie.AccountName;
            dto.name = dto.name ?? string.Empty;
            dto.sex = dto.sex ?? string.Empty;                                                       
            dto.age = dto.age ;
            dto.remark = dto.remark ?? string.Empty;
            _demoRepository.Insert(dto);
        }

        public void UpdateData(demo dto)
        {
            demo demo = _demoRepository.GetById(dto.id);
            dto.name = dto.name ?? string.Empty;
            dto.sex = dto.sex ?? string.Empty;
            dto.age = dto.age;
            dto.remark = dto.remark ?? string.Empty;
            dto.create_person = demo.create_person ?? string.Empty;
            dto.create_time = demo.create_time;
            _demoRepository.Update(dto);
        }

        public void DelUserByIds(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var idsArray = ids.Split(',');
                _demoRepository.DeleteById(idsArray);
            }
        }

        public demo GetDemoByIds(string id)
        {
            return _demoRepository.GetById(id);
        }
    }
}
