using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.ViewModel.SystemDto
{
    public class RoleMenuDto
    {
        public long id { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public int menu_sort { get; set; }
        public long parent_id { get; set; }
        public int menu_type { get; set; }
        public string menu_icon { get; set; }
    }
}
