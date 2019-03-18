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
    public class SysRoleController : BaseController
    {

        private readonly ISysRoleService _roleService;

        private readonly ISysLogService _logService;

        public SysRoleController(ISysRoleService roleService,ISysLogService logService)
        {
            _roleService = roleService;
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
            data = _roleService.GetData(param);
            _logService.WriteLog(LogType.VIEW, $"查询角色", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_role dto)
        {
            if (dto.id == 0)
            {
                _roleService.InsertRoleData(dto);
                _logService.WriteLog(LogType.ADD, $"添加角色(" + dto.role_name + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _roleService.UpdateRoleData(dto);
                _logService.WriteLog(LogType.EDIT, $"修改角色(" + dto.role_name + ")", LogState.NORMAL);//写入日志
            }
            return Success("保存成功");
        }

        public ActionResult GetRoleByIds(string id)
        {
            var result = new ResultAdaptDto();
            var data = _roleService.GetRoleById(id);
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取角色(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult DelRoleByIds(string ids)
        {
            _roleService.DelRoleByIds(ids);
            _logService.WriteLog(LogType.DEL, $"删除角色(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }

        public ActionResult SetRoleAuth(string id, string roleName)
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
            _logService.WriteLog(LogType.OTHER, $"获取角色(" + roleId + ")树结构", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult SaveRoleAuth(string roleId, string ids)
        {
            var result = new ResultAdaptDto();
            _roleService.SaveRoleAuth(roleId, ids);
            _logService.WriteLog(LogType.OTHER, $"编辑角色(" + roleId + ")权限", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult GetRoleMenuButton(string menuId)
        {
            var result = new ResultAdaptDto();
            return Content(result.ToJson());
        }
    }
}