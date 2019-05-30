using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.Model;
using Nzh.Hero.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Repository
{
    public class SysMenuRefOperateRepository : BaseRepository<sys_menu_ref_operate>, ISysMenuRefOperateRepository
    {
        public SysMenuRefOperateRepository(ISqlDbContext sqldb)
          : base(sqldb)
        {

        }
    }
}
