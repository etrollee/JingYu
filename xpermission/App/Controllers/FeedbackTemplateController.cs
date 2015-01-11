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

namespace BLL
{
    public class FeedbackTemplateController : BaseController
    {
        IFeedbackTemplateBLL iFeedbackTemplateBll;
        ISysPersonBLL _iSysPersonBll;

        ValidationErrors validationErrors = new ValidationErrors();

        public FeedbackTemplateController()
            : this(new FeedbackTemplateBLL(), new SysPersonBLL())
        {
        }

        public FeedbackTemplateController(FeedbackTemplateBLL bll, SysPersonBLL sysPersonBll)
        {
            iFeedbackTemplateBll = bll;
            _iSysPersonBll = sysPersonBll;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
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
            List<FeedbackTemplate> queryData = null;
            string syspersonId = GetCurrentAccount().Id;
            var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            queryData = iFeedbackTemplateBll.GetByParam(syspersonId, id, page, rows, order,
                    sort, search, ref total).Where(o => o.CreatePersonId == syspersonId || o.IsSys).ToList();

            return Json(new datagrid
            {
                total = total,
                rows = queryData.Select(s => new
                {
                    Id = s.Id,
                    CreatePersonId = s.CreatePersonId,
                    Name = s.Name,
                    IsSys = s.IsSys,
                    Description = s.Description
                })
            });
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Details(string id)
        {
            FeedbackTemplate item = iFeedbackTemplateBll.GetById(id);
            return View(item);

        }

        /// <summary>
        /// 首次创建
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Create(string id)
        {
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public ActionResult Create(FeedbackTemplate entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                entity.Id = Result.GetNewId();
                entity.CreatePersonId = GetCurrentAccount().Id;
                if (iFeedbackTemplateBll.Create(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，反馈模板的信息的Id为" + entity.Id, "反馈模板"
                       );//写入日志 
                    return Json(Suggestion.InsertSucceed);
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，反馈模板的信息，" + returnValue, "反馈模板"
                        );//写入日志                      
                    return Json(Suggestion.InsertFail + returnValue); //提示插入失败
                }
            }
            return Json(Suggestion.InsertFail + "，请核对输入的数据的格式"); //提示输入的数据的格式不对 
        }

        /// <summary>
        /// 首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns> 
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            FeedbackTemplate entity = iFeedbackTemplateBll.GetById(id);
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
        public ActionResult Edit(string id, FeedbackTemplate entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                if (iFeedbackTemplateBll.Edit(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，反馈模板信息的Id为" + id, "反馈模板"
                        );//写入日志                           
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，反馈模板信息的Id为" + id + "," + returnValue, "反馈模板"
                        );//写入日志                           
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

                if (iFeedbackTemplateBll.DeleteCollection(ref validationErrors, deleteId))
                {
                    LogClassModels.WriteServiceLog(Suggestion.DeleteSucceed + "，信息的Id为" + string.Join(",", deleteId), "消息"
                        );//删除成功，写入日志
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，信息的Id为" +
                        string.Join(",", deleteId) + "," + returnValue, "消息"
                        );//删除失败，写入日志
                }
            }

            return Json(returnValue);
        }
    }
}
