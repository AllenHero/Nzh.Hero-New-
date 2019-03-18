using Nzh.Hero.IService.Base;
using Nzh.Hero.Model;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.IService
{
    public interface ISysLogService 
    {

        BootstrapGridDto GetData(BootstrapGridDto param);

        void WriteLog(LogType logtype, string logmsg, LogState logstate);
    }
}
