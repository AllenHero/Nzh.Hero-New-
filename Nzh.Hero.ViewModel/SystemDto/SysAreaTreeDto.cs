using Nzh.Hero.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.ViewModel.SystemDto
{
    public class SysAreaTreeDto : sys_area
    {
        public List<SysAreaTreeDto> children { get; set; }
    }
}
