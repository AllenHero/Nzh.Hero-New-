using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysAreaController : BaseController
    {
        private readonly SysAreaService _areaService;

        public SysAreaController(SysAreaService areaService)
        {
            _areaService = areaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            ViewBag.Pid = RequestHelper.RequestGet("pid", "0");
            ViewBag.Pname = RequestHelper.RequestGet("pname", "");
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            var pid = RequestHelper.RequestGet("pid", "0");
            var list = _areaService.GetData(pid.ToInt64());
            return Content(list.ToJson());
        }

        public ActionResult GetDataById(string id)
        {
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }

        public ActionResult DelDataByIds(string ids)
        {
            _areaService.DelByIds(ids);
            return Success("删除成功");
        }

        public ActionResult GetCounty()
        {
            var pid = RequestHelper.RequestGet("pid", "0");
            var result = new ResultAdaptDto();
            var data = _areaService.GetCountys(pid.ToInt64());
            result.data.Add("conty", data);
            return Content(result.ToJson());
        }

        public ActionResult GetAreaSel()
        {
            var data = new List<CitySelDto>();
            data = _areaService.GetCitySel();
            return Content(data.ToJson());
        }

        public ActionResult GetAreaZtree()
        {
            string level = RequestHelper.RequestGet("level", "3");
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }

        public ActionResult GetProvince()
        {
            var result = new ResultAdaptDto();
            var data = _areaService.GetProvince();
            data.Insert(0, new ZtreeDto() { id = "0", name = "全国" });
            result.data.Add("list", data);
            return Content(result.ToJson());
        }
    }
}