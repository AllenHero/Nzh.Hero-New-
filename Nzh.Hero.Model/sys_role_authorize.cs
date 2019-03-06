using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_role_authorize
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public long role_id { get; set; }
        public long menu_id { get; set; }
        public long menu_pid { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
    }
}
