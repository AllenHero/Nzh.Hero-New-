using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;

namespace Nzh.Hero.Controllers.Admin
{
    [AuthorizeFilter]
    [WebExceptionFilter]
    public class BaseController : Controller
    {
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
    }
}