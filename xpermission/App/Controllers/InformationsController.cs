using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using DAL;
using IBLL;
using Models;

namespace App.Controllers
{
    public class InformationsController : BaseController
    {
        IInformationsBLL _iInformationBll;
        ISysPersonBLL _iSysPersonBll;
        ValidationErrors validationErrors = new ValidationErrors();

        public InformationsController()
            : this(new InformationsBLL(),new SysPersonBLL())
        {
        }

        public InformationsController(InformationsBLL bll, SysPersonBLL sysPersonBll)
        {
            _iInformationBll = bll;
            _iSysPersonBll = sysPersonBll;

        }

        Func<string, int, string> substring = (src, len) =>
        {
            if (string.IsNullOrEmpty(src))
            {
                return src;
            }
            var srcLen = src.Length;
            return src.Substring(0, len > srcLen ? srcLen : len);
        };

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
            List<Informations> queryData = null;
            string syspersonId = GetCurrentAccount().Id;
            queryData = _iInformationBll.GetByParam(syspersonId, id, page, rows, order, sort, search, ref total);
            var filterRegex = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return Json(new datagrid
            {
                total = total,
                rows = queryData.Select(s => new
                {
                    Id = s.Id,
                    Title = "<a href='/Informations/InformationFeedbackReport?informationId=" + s.Id + "'>" + s.Title + "</a>",
                    Content = substring(filterRegex.Replace(s.Content, ""), 20),
                    IsSend = s.IsSend.ToValue(),
                    FeedbackTemplateId = substring(s.FeedbackTemplateId, 20),
                    TimeLimit = s.TimeLimit,
                    MemberId = substring(s.MemberId, 20),
                    CreatePersonId = s.SysPerson.Name,
                    CreateTime = s.CreateTime.ToString(),
                    Type = ConvertToString(s.Type)
                })
            });
        }

        
        private string ConvertToString(int type) 
        {
            string result = string.Empty;
            if (type==1)
            {
                result = "普通信息";
            }
            else if (type == 2)
            {
                result = "政务信息";
            }
            else
            {
                result = "升级信息";
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="informationId"></param>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult InformationFeedbackReport(string informationId)
        {
            Session["ReportInformationId"] = informationId;
            var model = _iInformationBll.GetInformationFeedbackReport(informationId);
            //string server = null;
            //string syspersonId = GetCurrentAccount().Id;
            //var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            //if (sysRole.Power == 1 || sysRole.Power == 2)
            //{
            //    server = "server";
            //}
            //ViewBag.Power = server;

            return View(model);
        }

        /// <summary>
        /// 未反馈会员列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult GetUnFeedbackMemberData(string id, int page, int rows, string order, string sort, string search)
        {
            var informationId = Session["ReportInformationId"].ToString();
            var model = _iInformationBll.GetInformationFeedbackReport(informationId);
            var queryData=model.UnFeedbackTemlateMembersList;
            return Json(new datagrid
            {
                total = queryData.Count(),
                rows = queryData.Select(s => new 
                {
                    MemberName=s.MemberName,
                    Contacts=s.Contacts,
                    Phone=s.Phone
                })
            });
        }

        /// <summary>
        /// 已反馈会员列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult GetFeedbackMemberData(string id, int page, int rows, string order, string sort, string search)
        {
            var informationId = Session["ReportInformationId"].ToString();
            var model = _iInformationBll.GetInformationFeedbackReport(informationId);
            var queryData = model.FeedbackTemlateMembersList;
            return Json(new datagrid
            {
                total = queryData.Count(),
                rows = queryData.Select(s => new
                {
                    MemberName = s.MemberName,
                    Contacts = s.Contacts,
                    Phone = s.Phone,
                    FeedbackTemplateName=s.FeedbackTemplateName,
                    FeedbackContent = s.FeedbackContent,
                    CreateTime = s.CreateTime.ToString()
                })
            });
        }

        /// <summary>
        /// 未反馈商家列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult GetUnFeedbackMerchantData(string id, int page, int rows, string order, string sort, string search)
        {
            var informationId = Session["ReportInformationId"].ToString();
            var model = _iInformationBll.GetInformationFeedbackReport(informationId);
            var queryData = model.UnFeedbackTemlateMerchantsList;
            return Json(new datagrid
            {
                total = queryData.Count(),
                rows = queryData.Select(s => new
                {
                    MemberName = s.MemberName,
                    Contacts = s.Contacts,
                    Phone = s.Phone
                })
            });
        }

        /// <summary>
        /// 已反馈商家列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public JsonResult GetFeedbackMerchantData(string id, int page, int rows, string order, string sort, string search)
        {
            var informationId = Session["ReportInformationId"].ToString();
            var model = _iInformationBll.GetInformationFeedbackReport(informationId);
            var queryData = model.FeedbackTemlateMerchantsList;
            return Json(new datagrid
            {
                total = queryData.Count(),
                rows = queryData.Select(s => new
                {
                    MemberName = s.MemberName,
                    Contacts = s.Contacts,
                    Phone = s.Phone,
                    FeedbackTemplateName = s.FeedbackTemplateName,
                    FeedbackContent = s.FeedbackContent,
                    CreateTime = s.CreateTime.ToString()
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
            Informations item = _iInformationBll.GetById(id);
            return View(item);

        }

        /// <summary>
        /// 首次创建
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Create(string id)
        {
            //string server = null;
            //string syspersonId = GetCurrentAccount().Id;
            //var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            //if (sysRole.Power == 1 || sysRole.Power == 2)
            //{
            //    server = "server";
            //}
            //ViewBag.Power = server;

            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [SupportFilter]
        public ActionResult Create(Informations entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                entity.Id = Result.GetNewId();
                entity.CreatePersonId = GetCurrentAccount().Id;
                entity.CreateTime = DateTime.Now;
                entity.IsSend = false;
                if (_iInformationBll.Create(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，信息的信息的Id为" + entity.Id, "信息"
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，信息的信息，" + returnValue, "信息"
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
            //string server = null;
            //string syspersonId = GetCurrentAccount().Id;
            //var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            //if (sysRole.Power == 1 || sysRole.Power == 2)
            //{
            //    server = "server";
            //}
            //ViewBag.Power = server;
            Informations entity = _iInformationBll.GetById(id);

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
        public ActionResult Edit(Informations entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;

                if (_iInformationBll.Edit(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，信息信息的Id为" + entity.Id, "信息"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，信息信息的Id为" + entity.Id + "," + returnValue, "信息"
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

                if (_iInformationBll.DeleteCollection(ref validationErrors, deleteId))
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，信息的Id为" 
                        + string.Join(",", deleteId) + "," + returnValue, "消息"
                        );//删除失败，写入日志
                }
            }

            return Json(returnValue);
        }

        public ActionResult UpdateInfo()
        {
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [SupportFilter]
        public ActionResult UpdateInfo(Informations entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                entity.Id = Result.GetNewId();
                entity.CreatePersonId = GetCurrentAccount().Id;
                entity.CreateTime = DateTime.Now;
                entity.IsSend = false;
                entity.Type = 3;
                entity.TimeLimit = 10;
                if (_iInformationBll.AddUpdateInfo(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，信息的信息的Id为" + entity.Id, "信息"
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，信息的信息，" + returnValue, "信息"
                        );//写入日志                      
                    return Json(Suggestion.InsertFail + returnValue); //提示插入失败
                }
            }
            return Json(Suggestion.InsertFail + "，请核对输入的数据的格式"); //提示输入的数据的格式不对 
        }
    }
}
