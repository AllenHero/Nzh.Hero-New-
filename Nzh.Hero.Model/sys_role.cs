using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_role
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string role_name { get; set; }
        public string role_code { get; set; }
        public int sort { get; set; }
        public string remark { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
        public bool role_super { get; set; }
    }
}
