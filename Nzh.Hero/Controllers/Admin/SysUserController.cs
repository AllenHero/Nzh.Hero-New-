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
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysUserController : BaseController
    {
        private readonly ISysUserService _userService;

        public SysUserController(ISysUserService userService)
        {
            _userService = userService;
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
            }
            else
            {
                _userService.UpdateData(dto);
            }
            return Success();
        }

        public ActionResult GetUserById(string id)
        {
            var result = new ResultAdaptDto();
            var data = _userService.GetUserById(id);
            result.data.Add("model", data);
            return Content(result.ToJson());
        }

        public ActionResult DelUserByIds(string ids)
        {
            _userService.DelUserByIds(ids);
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
            return Success("修改成功");
        }
    }
}