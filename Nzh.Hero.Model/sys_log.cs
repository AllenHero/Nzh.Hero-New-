using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Model
{
    public class sys_log
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }
        public string operation { get; set; }
        public DateTime logtime { get; set; }
        public string logtype { get; set; }
        public string logmsg { get; set; }
        public string loglevel { get; set; }
        public string logip { get; set; }


    }
}
