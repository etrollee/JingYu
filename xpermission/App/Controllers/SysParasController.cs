using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using DAL;
using IBLL;
using Models;

namespace App.Controllers
{
    public class SysParasController : BaseController
    {
        ISysParasBll _sysParasBll;

        ValidationErrors validationErrors = new ValidationErrors();

        public SysParasController()
            : this( new SysParasBll())
        {
        }

        public SysParasController(SysParasBll sysParasBll)
        {
            _sysParasBll = sysParasBll;
        }

        public ActionResult Index()
        {
            var model = _sysParasBll.GetSysParas();

            return View(model);
        }

        /// <summary>
        /// 提交编辑信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="collection">客户端传回的集合</param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public ActionResult Edit(int id, SysParas entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                if (_sysParasBll.Edit(ref validationErrors, entity))
                {
                    ModelState.AddModelError("", "修改系统参数成功");
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，系统参数信息的Id为" + id, "系统参数"
                        );//写入日志                           
                    return View("Index") ; //提示更新成功 
                }
                else
                {
                    if (validationErrors != null && validationErrors.Count > 0)
                    {
                        validationErrors.All(a =>
                        {
                            returnValue += a.ErrorMessage;
                            return true;
                        });
                    }
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，系统参数信息的Id为" + id + "," + returnValue, "系统参数"
                        );//写入日志   
                    ModelState.AddModelError("", "修改系统参数失败，请核实数据");
                    return View("Index"); //提示更新失败
                }
            }
            return View("Index"); //提示输入的数据的格式不对  
        }

    }
}
