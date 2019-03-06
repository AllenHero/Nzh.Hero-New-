using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysFuncController : BaseController
    {
        private readonly ISysFuncService _funcService;

        public SysFuncController(ISysFuncService funcService)
        {
            _funcService = funcService;
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
            var data = new BootstrapGridDto();
            data = _funcService.GetData(param);
            return Content(data.ToJson());
        }

        public JsonResult SaveData(sys_operate dto)
        {
            if (dto.id == 0)
            {
                _funcService.InsertFunc(dto);
            }
            else
            {
                _funcService.UpdateFunc(dto);
            }
            return Json("");
        }

        public ActionResult GetDataById(string id)
        {
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }

        public ActionResult DelByIds(string ids)
        {
            _funcService.DelByIds(ids);
            return Success("删除成功");
        }
    }
}