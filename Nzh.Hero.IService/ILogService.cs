using Nzh.Hero.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ILogService
    {
        void WriteLog(sys_log dto);
    }
}
