using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;
using Nzh.Hero.IService;
using Nzh.Hero.Model;

namespace Nzh.Hero.Controllers.Admin
{
    [AuthorizeFilter]
    [WebExceptionFilter]
    public class BaseController : Controller
    {
        //private readonly ILogService _logService;

        //public BaseController(ILogService logService)
        //{
        //    _logService = logService;
        //}

        protected virtual ContentResult Success(string msg)
        {
            var result = new { statusCode = 200, msg = msg };
            return Content(result.ToJson());
        }
        protected virtual ContentResult Success()
        {
            var result = new { statusCode = 200 };
            return Content(result.ToJson());
        }
        protected virtual ContentResult Error()
        {
            var result = new { statusCode = 300, msg = "操作失败" };
            return Content(result.ToJson());
        }
        protected virtual ContentResult Error(string msg)
        {
            var result = new { statusCode = 300, msg = msg };
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult WriteLog(sys_log dto)
        {
            //_logService.WriteLog(dto);
            return Success("添加日志成功");
        }
    }
}