using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class MemberController : BaseController
    {
        IMemberBLL _iMemberBll;
        IMerchantBLL _iMerchantBll;
        IMemberGroupBLL _iMemberGroupBll;
        ISysPersonBLL _iSysPersonBll;

        ValidationErrors validationErrors = new ValidationErrors();

        public MemberController()
            : this(new MemberBLL(), new MerchantBLL(), new MemberGroupBLL(), new SysPersonBLL())
        {
        }

        public MemberController(MemberBLL memberBll, MerchantBLL merchantBll,
            MemberGroupBLL memberGroupBll, SysPersonBLL sysPersonBll)
        {
            _iMemberBll = memberBll;
            _iMerchantBll = merchantBll;
            _iMemberGroupBll = memberGroupBll;
            _iSysPersonBll = sysPersonBll;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Index()
        {

            string syspersonId = GetCurrentAccount().Id;
            var memberGroups = _iMemberGroupBll.GetAll().Where(o => o.CreatePersonId == syspersonId).ToList();
            ViewBag.MemberGroups = new SelectList(memberGroups, "Id", "Name");
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
            var filterRegex = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Func<string, int, string> substring = (src, len) =>
            {
                if (string.IsNullOrEmpty(src))
                {
                    return src;
                }
                var srcLen = src.Length;
                return src.Substring(0, len > srcLen ? srcLen : len);
            };

            int total = 0;
            List<Member> queryData = null;
            string syspersonId = GetCurrentAccount().Id;
            var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            queryData = _iMemberBll.GetByParam(syspersonId, id, page, rows, order, sort, search, ref total);

            try
            {
                if (sysRole.Power == 3)
                {
                    var data = queryData.Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        RegisterCode = s.CreatePersonId == syspersonId || s.IsVisible ? s.RegisterCode : "",
                        Area = s.CreatePersonId == syspersonId || s.IsVisible ? s.Area : "",
                        Regtime = s.CreatePersonId == syspersonId || s.IsVisible ?
                        (s.Regtime.GetDateTime().Date == DateTime.Now.Date ? null : s.Regtime.GetDateTime().Date.ToString()) : null,
                        RegisteredCapital = s.CreatePersonId == syspersonId || s.IsVisible ?
                        (s.RegisteredCapital == 0 ? null : s.RegisteredCapital) : null,
                        Contacts = s.CreatePersonId == syspersonId || s.IsVisible ? s.Contacts : "",
                        Phone = s.CreatePersonId == syspersonId || s.IsVisible ? s.Phone : "",
                        RegisteredAddress = s.CreatePersonId == syspersonId || s.IsVisible ? s.RegisteredAddress : "",
                        LegalRepresentative = s.CreatePersonId == syspersonId || s.IsVisible ? s.LegalRepresentative : "",
                        RegisteredCellPhone = s.CreatePersonId == syspersonId || s.IsVisible ? s.RegisteredCellPhone : "",
                        SelfDefineOne = s.CreatePersonId == syspersonId || s.IsVisible ? s.SelfDefineOne : "",
                        SelftDefineTwo = s.CreatePersonId == syspersonId || s.IsVisible ? s.SelfDefineTwo : "",
                        Remark = s.CreatePersonId == syspersonId || s.IsVisible ? substring(s.Remark, 20) : "",
                        VIP = s.VIP.ToValue(),
                        IsValid = s.IsValid.ToValue(),
                        LogOnTimes = s.LogOnTimes,
                        MemberGroupId = s.CreatePersonId == syspersonId || s.IsVisible ? s.MemberGroupId : ""

                    });

                    return Json(new datagrid
                    {
                        total = total,
                        rows = data
                    });
                }
                else
                {
                    var data = queryData.Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        RegisterCode = s.RegisterCode,
                        Area = s.Area,
                        Regtime = s.Regtime.GetDateTime().Date == DateTime.Now.Date ? null : s.Regtime.GetDateTime().Date.ToString(),
                        RegisteredCapital = s.RegisteredCapital == 0 ? null : s.RegisteredCapital,
                        Contacts = s.Contacts,
                        Phone = s.Phone,
                        RegisteredAddress = s.RegisteredAddress,
                        LegalRepresentative = s.LegalRepresentative,
                        RegisteredCellPhone = s.RegisteredCellPhone,
                        SelfDefineOne = s.SelfDefineOne,
                        SelftDefineTwo = s.SelfDefineTwo,
                        Remark = substring(s.Remark, 20),
                        VIP = s.VIP.ToValue(),
                        IsValid = s.IsValid.ToValue(),
                        LogOnTimes = s.LogOnTimes,
                        MemberGroupId = s.MemberGroupId

                    });
                    return Json(new datagrid
                    {
                        total = total,
                        rows = data
                    });
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Details(string id)
        {
            Member item = _iMemberBll.GetById(id);
            string syspersonId = GetCurrentAccount().Id;
            var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            bool isVisible = true;
            if (item.CreatePersonId != syspersonId && sysRole.Power == 3)
            {
                isVisible = item.IsVisible;
            }
            ViewBag.IsVisible = isVisible;

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
        public ActionResult Create(Member entity)
        {
            var sysPeopleId = GetCurrentAccount().Id;
            var merchantId = _iMerchantBll.GetMerchantId(sysPeopleId);
            var isVasible = false;
            if (!string.IsNullOrEmpty(merchantId))
            {
                var merchant = _iMerchantBll.GetById(merchantId);
                isVasible = merchant.IsVisible == 1 ? true : false;
            }
            entity.IsVisible = isVasible;
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                entity.Id = Result.GetNewId();
                entity.CreatePersonId = sysPeopleId;
                entity.IsValid = true;
                if (_iMemberBll.Create(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，会员的信息的Id为" + entity.Id, "会员"
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，会员的信息，" + returnValue, "会员"
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
            Member entity = _iMemberBll.GetById(id);
            entity.OldRegisterCode = entity.RegisterCode;
            string syspersonId = GetCurrentAccount().Id;
            var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            bool isVisible = true;
            if (entity.CreatePersonId != syspersonId && sysRole.Power == 3)
            {
                isVisible = entity.IsVisible;
            }
            ViewBag.IsVisible = isVisible;
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
        public ActionResult Edit(string id, Member entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                string syspersonId = GetCurrentAccount().Id;
                if (_iMemberBll.Edit(syspersonId, ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，会员的Id为" + id, "会员"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，会员的Id为" + id + "," + returnValue, "会员"
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
                var personId = GetCurrentAccount().Id;
                if (_iMemberBll.DeleteCollection(personId, ref validationErrors, deleteId))
                {
                    LogClassModels.WriteServiceLog(Suggestion.DeleteSucceed + "，会员的Id为" + string.Join(",", deleteId), "会员"
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，会员的Id为"
                        + string.Join(",", deleteId) + "," + returnValue, "会员"
                        );//删除失败，写入日志
                }
            }

            return Json(returnValue);
        }

        /// <summary>
        /// 首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns> 
        [SupportFilter]
        public ActionResult Setup(string id)
        {
            Member entity = _iMemberBll.GetById(id);
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
        public ActionResult Setup(Member entity)
        {
            if (entity != null)
            {
                string returnValue = string.Empty;
                if (_iMemberBll.Setup(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，会员的Id为" + entity.Id, "会员"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，会员的Id为" + entity.Id + "," + returnValue, "会员"
                        );//写入日志                           
                    return Json(Suggestion.UpdateFail + returnValue); //提示更新失败
                }
            }
            return Json(Suggestion.UpdateFail + "请核对输入的数据的格式"); //提示输入的数据的格式不对  
        }
    }
}
