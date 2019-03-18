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
using Nzh.Hero.ViewModel.Enum;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysFuncController : BaseController
    {
        private readonly ISysFuncService _funcService;

        private readonly ILogService _logService;

        public SysFuncController(ISysFuncService funcService, ILogService logService)
        {
            _funcService = funcService;
            _logService = logService;
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
            _logService.WriteLog(LogType.VIEW, $"查询操作", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        public JsonResult SaveData(sys_operate dto)
        {
            if (dto.id == 0)
            {
                _funcService.InsertFunc(dto);
                _logService.WriteLog(LogType.ADD, $"添加操作(" + dto.func_cname + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _funcService.UpdateFunc(dto);
                _logService.WriteLog(LogType.EDIT, $"修改操作(" + dto.func_cname + ")", LogState.NORMAL);//写入日志
            }
            return Json("保存成功");
        }

        public ActionResult GetFuncByIds(string id)
        {
            var result = new ResultAdaptDto();
            var data = _funcService.GetFuncByIds(id);
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取操作(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult DelByIds(string ids)
        {
            _funcService.DelByIds(ids);
            _logService.WriteLog(LogType.DEL, $"删除操作(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }
    }
}