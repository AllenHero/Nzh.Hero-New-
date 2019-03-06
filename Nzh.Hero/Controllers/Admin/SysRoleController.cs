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
    public class SysRoleController : BaseController
    {

        private readonly ISysRoleService _roleService;

        public SysRoleController(ISysRoleService roleService)
        {
            _roleService = roleService;
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
            data = _roleService.GetData(param);
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_role dto)
        {
            if (dto.id == 0)
            {
                _roleService.InsertRoleData(dto);
            }
            else
            {
                _roleService.UpdateRoleData(dto);
            }
            return Success();
        }

        public ActionResult GetRoleByIds(string id)
        {
            var result = new ResultAdaptDto();
            var data = _roleService.GetRoleById(id);
            result.data.Add("model", data);
            return Content(result.ToJson());
        }

        public ActionResult DelRoleByIds(string ids)
        {
            _roleService.DelRoleByIds(ids);
            return Success("删除成功");
        }

        public ActionResult SetRoleAuth(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult SetRoleAuthNew(string id, string roleName)
        {
            ViewBag.Id = id;
            ViewBag.RoleName = roleName;
            return View();
        }

        public ActionResult GetRoleAuthById(string roleId)
        {
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }

        public ActionResult GetRoleMenuTree(string roleId)
        {
            var result = new ResultAdaptDto();
            var data = _roleService.GetRoleMenuTree(roleId);
            result.data.Add("roleMenu", data);
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult SaveRoleAuth(string roleId, string ids)
        {
            var result = new ResultAdaptDto();
            _roleService.SaveRoleAuth(roleId, ids);
            return Content(result.ToJson());
        }

        public ActionResult GetRoleMenuButton(string menuId)
        {
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }
    }
}