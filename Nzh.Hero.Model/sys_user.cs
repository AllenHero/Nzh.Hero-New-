using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_user
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string account_name { get; set; }
        public string real_name { get; set; }
        public string pass_word { get; set; }
        public string mobile_phone { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
        public long province { get; set; }
        public int city { get; set; }
        public int county { get; set; }
        public bool is_super { get; set; }
        public long sys_role_id { get; set; }
        public int user_level { get; set; }
        public string remark { get; set; }
    }
}
