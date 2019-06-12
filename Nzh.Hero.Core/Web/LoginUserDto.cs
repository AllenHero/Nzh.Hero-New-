﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.Core.Web
{
    public class LoginUserDto
    {
        public long Id { get; set; }
        public string AccountName { get; set; }
        public string RealName { get; set; }
        public bool IsSuper { get; set; }
        public long SysRoleId { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public int County { get; set; }
        public int UserLevel { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string IP { get; set; }
    }
}
