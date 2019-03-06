using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Core.DbContext
{
    public interface ISqlDbContext
    {
        SqlSugarClient DbInstance();
    }
}
