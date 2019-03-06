using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_menu
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public int menu_sort { get; set; }
        public long parent_id { get; set; }
        public string remark { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
        public int menu_level { get; set; }
        public int menu_type { get; set; }
        public bool iframe { get; set; }
        public string menu_icon { get; set; }
    }
}
