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
    public class DemoController : BaseController
    {
        private readonly IDemoService _demoService;

        private readonly ILogService _logService;

        public DemoController(IDemoService demoService, ILogService logService) 
        {
            _demoService = demoService;
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetTestByIds(string id)
        {
            var result = new ResultAdaptDto();
            var data = _demoService.GetTestById(id);
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取demo(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult GetData(BootstrapGridDto param)
        {
            var data = _demoService.GetData(param);
            _logService.WriteLog(LogType.VIEW, $"查询demo", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(demo dto)
        {
            if (dto.id == 0)
            {
                _demoService.InsertData(dto);
                _logService.WriteLog(LogType.ADD, $"添加demo(" + dto.name + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _demoService.UpdateData(dto);
                _logService.WriteLog(LogType.EDIT, $"修改demo(" + dto.name + ")", LogState.NORMAL);//写入日志
            }
            return Success("保存成功");
        }

        public ActionResult DelUserByIds(string ids)
        {
            _demoService.DelUserByIds(ids);
            _logService.WriteLog(LogType.DEL, $"删除demo(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }
    }
}