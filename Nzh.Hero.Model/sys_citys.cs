using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_citys
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public int zipcode { get; set; }
        public string name { get; set; }
        public int province_code { get; set; }
        public int city_code { get; set; }
        public int city_level { get; set; }
    }
}
