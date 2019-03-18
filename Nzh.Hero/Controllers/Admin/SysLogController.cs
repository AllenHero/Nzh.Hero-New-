using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Common.Security;
using Nzh.Hero.Core.Web;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.Enum;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysLogController : BaseController
    {

        private readonly ISysLogService _logService;

        public SysLogController(ISysLogService logService) 
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(BootstrapGridDto param)
        {
            var data = _logService.GetData(param);
            _logService.WriteLog(LogType.VIEW, $"查询日志", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }
    }
}