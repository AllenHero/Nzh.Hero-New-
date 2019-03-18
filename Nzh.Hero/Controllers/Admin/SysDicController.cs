using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SysDicController : BaseController
    {
        private readonly ISysDicService _dicService;

        private readonly ISysLogService _logService;

        public SysDicController(ISysDicService dicService, ISysLogService logService)
        {
            _dicService = dicService;
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            var pid = RequestHelper.RequestGet("pid", "0");
            var dicSel = _dicService.GetDicSelList();
            dicSel.Insert(0, new sys_dictionary() { id = 0, dic_name = "请选择" });
            ViewBag.DicSelList = new SelectList(dicSel, "id", "dic_name", pid);
            return View();
        }

        public ActionResult GetData()
        {
            var data = new BootstrapGridDto();
            var list = _dicService.GetData();
            _logService.WriteLog(LogType.VIEW, $"查询字典", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        public ActionResult GetGridDataBypId()
        {
            string pid = RequestHelper.RequestGet("pid", "0");
            var list = _dicService.GetGridDataBypId(pid.ToInt64());
            _logService.WriteLog(LogType.OTHER, $"获取字典详细信息", LogState.NORMAL);//写入日志
            return Content(list.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_dictionary dto)
        {
            dto.dic_code = dto.dic_code.Trim();
            dto.dic_name = dto.dic_name.Trim();
            dto.dic_value = dto.dic_name;
            if (dto.id == 0)
            {
                _dicService.InsertDicData(dto);
                _logService.WriteLog(LogType.ADD, $"添加字典(" + dto.dic_name + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _dicService.UpdateDicData(dto);
                _logService.WriteLog(LogType.EDIT, $"修改字典(" + dto.dic_name + ")", LogState.NORMAL);//写入日志
            }
            return Success("保存成功");
        }

        public ActionResult GetDicById(string id)
        {
            var result = new ResultAdaptDto();
            var data = _dicService.GetDicById(id);
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取字典(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult del(string ids)
        {
            _dicService.DelByIds(ids);
            _logService.WriteLog(LogType.DEL, $"删除字典(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }

        public ActionResult GetDicZtree()
        {
            var result = new ResultAdaptDto();
            var data = _dicService.GetDicZtree();
            data.Insert(0, new ZtreeDto() { id = "0", name = "通用字典" });
            result.data.Add("dicTree", data);
            _logService.WriteLog(LogType.OTHER, $"获取字典树结构", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }   

        public ActionResult GetTreeGrid()
        {
            var list = _dicService.GetTreeGrid();
            return Content(list.ToJson());
        }
    }
}