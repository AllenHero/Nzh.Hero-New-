using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_setting
    {
        [SugarColumn(IsPrimaryKey = true)]
        public Int64 id { get; set; }
        public Int64 min_num { get; set; }
        public Int64 max_num { get; set; }
        public int set_type { get; set; }
        public string create_person { get; set; }
        public DateTime create_time { get; set; }
        public int ds_warn { get; set; }
        public int ds_pd { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public decimal step_warn_percent { get; set; }
        public DateTime last_count_time { get; set; }
    }
}
