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
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class DemoController : BaseController
    {
        private readonly IDemoService _demoService;

        public DemoController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult GetData(BootstrapGridDto param)
        {
            var data = _demoService.GetData(param);
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(demo dto)
        {
            if (dto.id == 0)
            {
                _demoService.InsertData(dto);
            }
            else
            {
                _demoService.UpdateData(dto);
            }
            return Success();
        }

        public ActionResult DelUserByIds(string ids)
        {
            _demoService.DelUserByIds(ids);
            return Success("删除成功");
        }
    }
}