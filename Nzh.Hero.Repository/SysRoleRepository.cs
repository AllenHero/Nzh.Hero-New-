using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.Model;
using Nzh.Hero.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Repository
{
    public class SysRoleRepository : BaseRepository<sys_role>, ISysRoleRepository
    {
        public SysRoleRepository(ISqlDbContext sqldb)
          : base(sqldb)
        {

        }
    }
}
