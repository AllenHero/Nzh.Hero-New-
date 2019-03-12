using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class demo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string remark { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
    }
}
