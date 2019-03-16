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
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysDicController : BaseController
    {
        private readonly ISysDicService _dicService;

        public SysDicController(ISysDicService dicService)
        {
            _dicService = dicService;
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
            return Content(data.ToJson());
        }

        public ActionResult GetGridDataBypId()
        {
            string pid = RequestHelper.RequestGet("pid", "0");
            var list = _dicService.GetGridDataBypId(pid.ToInt64());
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
            }
            else
            {
                _dicService.UpdateDicData(dto);
            }
            return Success("保存成功");
        }

        public ActionResult GetDicById(string id)
        {
            var result = new ResultAdaptDto();
            var data = _dicService.GetDicById(id);
            result.data.Add("model", data);
            return Content(result.ToJson());
        }

        public ActionResult del(string ids)
        {
            _dicService.DelByIds(ids);
            return Success("删除成功");
        }

        public ActionResult GetDicZtree()
        {
            var result = new ResultAdaptDto();
            var data = _dicService.GetDicZtree();
            data.Insert(0, new ZtreeDto() { id = "0", name = "通用字典" });
            result.data.Add("dicTree", data);
            return Content(result.ToJson());
        }

        public ActionResult TreeView()
        {
            return View();
        }

        public ActionResult GetTreeGrid()
        {
            var list = _dicService.GetTreeGrid();
            return Content(list.ToJson());
        }

        public ActionResult FormTest()
        {
            return View();
        }
    }
}