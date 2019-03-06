using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysMenuController : BaseController
    {
        private SysMenuService _menuService;

        public SysMenuController(SysMenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            var menuList = _menuService.GetMenuList().Where(s => s.menu_type == 0).OrderBy(s => s.menu_level).ToList();
            menuList.Insert(0, new sys_menu() { id = 0, menu_name = "请选择" });
            ViewBag.MenuSel = new SelectList(menuList, "id", "menu_name");
            //按钮
            //var funcSelList = _menuApp.GetFuncSelList();
            //funcSelList.Insert(0, new sys_operate(){ id = 0, func_cname = "请选择" });
            //ViewBag.FuncSel = new SelectList(funcSelList, "id", "func_cname");
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            var data = _menuService.GetMenuList();
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_menu dto)
        {
            if (dto.parent_id == 0)
            {
                dto.menu_url = string.Empty;
                dto.menu_icon = dto.menu_icon ?? "fa fa-desktop";
            }
            else
            {
                dto.menu_url = dto.menu_url ?? string.Empty;
                dto.menu_icon = dto.menu_icon ?? "fa fa-tag";
            }
            if (dto.id == 0)
            {
                _menuService.AddMenu(dto, string.Empty);
            }
            else
            {
                _menuService.UpdateMenu(dto, string.Empty);
            }
            return Success("保存成功");
        }

        [HttpGet]
        public ActionResult GetDataById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Error("参数错误");
            }
            var data = _menuService.GetMenuById(id);
            var result = new ResultAdaptDto();
            result.data.Add("model", data);
            return Content(result.ToJson());
        }

        [HttpGet]
        public ActionResult Del(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Error("参数错误");
            }
            _menuService.DelByIds(ids);
            return Success("删除成功");
        }
    }
}