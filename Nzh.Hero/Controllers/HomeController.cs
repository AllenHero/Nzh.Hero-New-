using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;
using Nzh.Hero.Models;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers
{
    public class HomeController : Controller
    {
        private readonly SysMenuService _menuService;

        public HomeController(SysMenuService menuService)
        {
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            var user = CookieHelper.GetUserLoginCookie();
            if (user != null)
            {
                var userDto = user.ToObject<LoginUserDto>();
                ViewBag.Id = userDto.Id.ToString();
                ViewBag.AccountName = userDto.AccountName;
                ViewBag.UserName = userDto.RealName;
            }
            return View();
        }

        public async Task<ActionResult> GetRoleMenu()
        {
            var user = CookieHelper.GetUserLoginCookie();
            var userDto = user.ToObject<LoginUserDto>();
            var result = new ResultAdaptDto();
            var menu = await _menuService.GetRoleMenu();
            //var status = GetStatusDic();
            result.data.Add("menu", menu);
            result.data.Add("user", userDto);
            // result.data.Add("status", status);
            return Content(result.ToJson());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
