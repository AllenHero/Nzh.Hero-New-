using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.Model;
using Nzh.Hero.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Repository
{
    public class SysMenuRepository : BaseRepository<sys_menu>, ISysMenuRepository
    {
        public SysMenuRepository(ISqlDbContext sqldb)
          : base(sqldb)
        {

        }
    }
}
