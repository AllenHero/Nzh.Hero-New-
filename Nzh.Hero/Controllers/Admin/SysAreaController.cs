using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.Enum;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysAreaController : BaseController
    {
        private readonly ISysAreaService _areaService;

        private readonly ISysLogService _logService;

        public SysAreaController(ISysAreaService areaService, ISysLogService logService)
        {
            _areaService = areaService;
            _logService = logService;
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
            _logService.WriteLog(LogType.VIEW, $"查询行政区域", LogState.NORMAL);//写入日志
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
            _logService.WriteLog(LogType.DEL, $"删除行政区域(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }

        public ActionResult GetCounty()
        {
            var pid = RequestHelper.RequestGet("pid", "0");
            var result = new ResultAdaptDto();
            var data = _areaService.GetCountys(pid.ToInt64());
            result.data.Add("conty", data);
            _logService.WriteLog(LogType.OTHER, $"获取城市树结构", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult GetAreaSel()
        {
            var data = new List<CitySelDto>();
            data = _areaService.GetCitySel();
            _logService.WriteLog(LogType.OTHER, $"获取地区树结构", LogState.NORMAL);//写入日志
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
            _logService.WriteLog(LogType.VIEW, $"获取省份树结构", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }
    }                                                     
}