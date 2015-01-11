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
    public class AppointmentController : BaseController
    {
        IAppointmentBLL _iAppointmentBll;
        ISysPersonBLL _iSysPersonBll;
        ValidationErrors validationErrors = new ValidationErrors();

        public AppointmentController()
            : this(new AppointmentBLL(), new SysPersonBLL())
        {
        }

        public AppointmentController(AppointmentBLL appointmentBll, SysPersonBLL sysPersonBll)
        {
            _iAppointmentBll = appointmentBll;
            _iSysPersonBll = sysPersonBll;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每页显示的行数</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult GetData(string id, int page, int rows, string order, string sort, string search)
        {

            int total = 0;
            List<Appointment> queryData = null;
            string syspersonId = GetCurrentAccount().Id;

            queryData = _iAppointmentBll.GetByParam(syspersonId, Convert.ToInt32(id), page, rows, 
                order, sort, search, ref total);
            return Json(new datagrid
            {
                total = total,
                rows = queryData.Select(s => new
                {
                    Id = s.Id,
                    MemberName = s.MemberName,
                    ServiceProductId = s.ServiceProduct.Name,
                    CreateTime = s.CreateTime.ToString(),
                    State = s.State.ToValue(),

                })
            });
        }

        /// <summary>
        /// 首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns> 
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            var entity = _iAppointmentBll.GetById(Convert.ToInt32(id));
            return View(entity);
        }

        /// <summary>
        /// 提交编辑信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="collection">客户端传回的集合</param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public ActionResult Edit(Appointment entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;

                if (_iAppointmentBll.Edit(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed +
                        "，信息信息的Id为" + entity.Id, "信息");//写入日志                           
                    return Json(Suggestion.UpdateSucceed); //提示更新成功 
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，预约的Id为" +
                        entity.Id + "," + returnValue, "预约");//写入日志                           
                    return Json(Suggestion.UpdateFail + returnValue); //提示更新失败
                }
            }
            return Json(Suggestion.UpdateFail + "请核对输入的数据的格式"); //提示输入的数据的格式不对  
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>   
        [HttpPost]
        [SupportFilter]
        public ActionResult Delete(FormCollection collection)
        {
            string returnValue = string.Empty;
            string[] deleteId = collection["query"].GetString().Split(',');
            if (deleteId != null && deleteId.Length > 0)
            {
                if (_iAppointmentBll.DeleteCollection(ref validationErrors, deleteId))
                {
                    LogClassModels.WriteServiceLog(Suggestion.DeleteSucceed + "，预约的Id为" +
                        string.Join(",", deleteId), "预约");//删除成功，写入日志
                    return Json("OK");
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，预约的Id为" +
                        string.Join(",", deleteId) + "," + returnValue, "预约"
                        );//删除失败，写入日志
                }
            }
            return Json(returnValue);
        }
    }
}
