using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Core.DbContext
{
    public class SqlDbContext :ISqlDbContext, IDisposable
    {
        public SqlSugarClient _db;
        public SqlSugarClient DbInstance()
        {
            if (_db == null && SugarDbConn.DbConnectStr != null)
            {
                _db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = SugarDbConn.DbConnectStr,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = false,
                    InitKeyType = InitKeyType.Attribute,
                });

            }
            return _db;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}
