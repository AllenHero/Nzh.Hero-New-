﻿using System;
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
    public class SysUserController : BaseController
    {
        private readonly ISysUserService _userService;

        private readonly ISysLogService _logService;


        public SysUserController(ISysUserService userService, ISysLogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            var role = _userService.GetRoleList();
            role.Insert(0, new sys_role() { id = 0, role_name = "请选择" });
            ViewBag.RoleList = new SelectList(role, "id", "role_name");
            return View();
        }

        public ActionResult GetAllUser()
        {
            var data = _userService.GetAllUser();
            return Content(data.ToJson());
        }

        public ActionResult GetData(BootstrapGridDto param)
        {
            string accountName = RequestHelper.RequestGet("accountName", "");
            var data = _userService.GetData(param, accountName);
            _logService.WriteLog(LogType.VIEW, $"查询用户", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_user dto)
        {
            var exist = _userService.CheckUserName(dto.account_name, dto.id);
            if (exist)
            {
                return Error("用户名已存在");
            }
            dto.pass_word = Encrypt.DesEncrypt(dto.pass_word.Trim());
            if (dto.id == 0)
            {
                _userService.InsertData(dto);
                _logService.WriteLog(LogType.ADD, $"添加用户(" + dto.account_name + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _userService.UpdateData(dto);
                _logService.WriteLog(LogType.EDIT, $"修改用户(" + dto.account_name + ")", LogState.NORMAL);//写入日志
            }
            return Success("保存成功");
        }

        public ActionResult GetUserById(string id)
        {
            var result = new ResultAdaptDto();
            var data = _userService.GetUserById(id);
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取用户(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        public ActionResult DelUserByIds(string ids)
        {
            _userService.DelUserByIds(ids);
            _logService.WriteLog(LogType.DEL, $"添加用户(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }

        public ActionResult Profile(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult UpdateProfile(sys_user dto)
        {
            dto.pass_word = Encrypt.DesEncrypt(dto.pass_word.Trim());
            _userService.UpdateProfile(dto);
            _logService.WriteLog(LogType.OTHER, $"修改用户(" + dto.account_name + ")信息", LogState.NORMAL);//写入日志
            return Success("修改成功");
        }
    }
}