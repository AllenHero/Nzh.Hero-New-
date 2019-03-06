﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nzh.Hero.Common.Extends;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Core.Web;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Common;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysAreaController : BaseController
    {
        private readonly SysAreaService _areaService;

        public SysAreaController(SysAreaService areaService)
        {
            _areaService = areaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            ViewBag.Pid = RequestHelper.RequestGet("pid", "0");
            ViewBag.Pname = RequestHelper.RequestGet("pname", "");
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            var pid = RequestHelper.RequestGet("pid", "0");
            var list = _areaService.GetData(pid.ToInt64());
            return Content(list.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_citys dto)
        {
            //bool existCount =false;
            //if (!string.IsNullOrEmpty(dto.zipcode))
            //{
            //    existCount = _areaApp.IsExist(dto.area_code, dto.id);
            //}
            //if (existCount)
            //{
            //    return Error("当前行政编码已经存在");
            //}
            //if (dto.id == 0)
            //{
            //    _areaApp.InsertData(dto);
            //}
            //else
            //{
            //    _areaApp.UpdateData(dto);
            //}
            return Success();
        }

        public ActionResult GetDataById(string id)
        {
            var result = new ResultAdaptDto();
            //result.Data = _areaApp.GetAreaById(id);
            ////result.statusCodeCode = JuiJsonEnum.Ok;
            return Content(result.ToJson());
        }

        public ActionResult DelDataByIds(string ids)
        {
            _areaService.DelByIds(ids);
            return Success("删除成功");
        }

        public ActionResult GetCounty()
        {
            var pid = RequestHelper.RequestGet("pid", "0");
            var result = new ResultAdaptDto();
            var data = _areaService.GetCountys(pid.ToInt64());
            result.data.Add("conty", data);
            return Content(result.ToJson());
        }

        public ActionResult GetAreaSel()
        {
            var data = new List<CitySelDto>();
            data = _areaService.GetCitySel();
            return Content(data.ToJson());
        }

        public ActionResult GetAreaZtree()
        {
            string level = RequestHelper.RequestGet("level", "3");
            var result = new ResultAdaptDto();
            //result.statusCode = true;
            //result.Data = _areaApp.GetAreaZtree(level.ToInt());
            //var data = _areaApp.GetAreaZtree();
            return Content(result.ToJson());
        }

        public ActionResult GetProvince()
        {
            var result = new ResultAdaptDto();
            var data = _areaService.GetProvince();
            data.Insert(0, new ZtreeDto() { id = "0", name = "全国" });
            result.data.Add("list", data);
            return Content(result.ToJson());
        }

        public ActionResult GetArea(string pid)
        {
            // var jresult = new JuiJsonResult();
            //var result = TencentMapHelper.ReqMapAreaData(pid.Trim());
            //if (string.IsNullOrEmpty(result))
            //{
            //    j////result.statusCodeCode = JuiJsonEnum.Error;
            //    jresult.message = "接口错误";
            //    return Content(jresult.ToJson());
            //}
            //var mobj = JObject.Parse(result);
            //List<MapAreaDto> list=new List<MapAreaDto>();
            //if (mobj["status"].ToString() == "0")
            //{
            //    if (mobj["result"] != null)
            //    {
            //        JArray resultobj = JArray.Parse(mobj["result"].ToString());
            //        JToken[] templist = resultobj[0].ToArray();
            //        int arrayCount = templist.Length;
            //        if (arrayCount > 0)
            //        {
            //            for (int i = 0; i < arrayCount; i++)
            //            {
            //                var model=new MapAreaDto();
            //                model.id = templist[i]["id"].ToString();
            //                model.fullname= templist[i]["fullname"].ToString();
            //                if (templist[i]["name"] != null)
            //                {
            //                    model.name = templist[i]["name"].ToString();
            //                }
            //                model.lat = templist[i]["location"]["lat"].ToString();
            //                model.lng = templist[i]["location"]["lng"].ToString();
            //                list.Add(model);
            //            }
            //        }
            //       // List<MapAreaDto> list = JsonConvert.DeserializeObject<List<MapAreaDto>>();
            //    }
            //}

            //if (list.Any())
            //{
            //    _areaApp.SaveTecentAreaData(list,pid);
            //}
            return Content("");
        }
    }
}