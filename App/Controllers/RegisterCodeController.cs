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
    public class RegisterCodeController : BaseController
    {
        IRegisterCodeBLL _iRegisterCodeBll;

        ValidationErrors validationErrors = new ValidationErrors();

        public RegisterCodeController()
            : this(new RegisterCodeBLL())
        { }

        public RegisterCodeController(RegisterCodeBLL registerCodeBll)
        {
            _iRegisterCodeBll = registerCodeBll;
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
            List<RegisterCode> queryData = _iRegisterCodeBll.GetByParam(Convert.ToInt32(id), page, rows, 
                order, sort, search, ref total);
            var data = queryData.Select(s => new 
            {
                Id=s.Id,
                Value=s.Value,
                CreateTime=s.CreateTime.ToString(),
                IsUsed=s.IsUsed.ToValue(),
                IsDistribution=s.IsDistribution.ToValue(),
                IsValid=s.IsValid.ToValue(),
                BelongMerchant = s.BelongMerchant,
                MemberName =s.MemberName,
                MerchantName = s.MerchantName
            });

            return Json(new datagrid 
            {
                total=total,
                rows=data
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
            RegisterCode item = _iRegisterCodeBll.GetById(Convert.ToInt32(id));
            return View(item);

        }

        /// <summary>
        /// 首次创建
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Create()
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
       // public ActionResult Create(int count)
        public ActionResult Create(RegisterCode entity)
        {
            if (entity !=null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                if (_iRegisterCodeBll.Create(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，注册码", "注册码");//写入日志 
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，注册码的信息，" + returnValue, "注册码"
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
            RegisterCode entity = _iRegisterCodeBll .GetById(Convert.ToInt32(id));
            if (!string.IsNullOrEmpty(entity.Name))
            {
                ViewBag.Name = entity.Name;
            }
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
        public ActionResult Edit(RegisterCode entity)
        {
            if (entity != null)
            {
                string returnValue = string.Empty;

                if (_iRegisterCodeBll.Edit(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，注册码的Id为" + entity.Id, "注册码"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，注册码的Id为" +
                        entity.Id + "," + returnValue, "注册码"
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

                if (_iRegisterCodeBll.DeleteCollection(ref validationErrors, deleteId))
                {
                    LogClassModels.WriteServiceLog(Suggestion.DeleteSucceed + "，注册码的Id为" + 
                        string.Join(",", deleteId), "注册码"
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，注册码的Id为" + 
                        string.Join(",", deleteId) + "," + returnValue, "注册码"
                        );//删除失败，写入日志
                }
            }

            return Json(returnValue);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult RegisterCodeLogin()
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
        public JsonResult GetRegisterCodeLoginData(string id, int page, int rows, string order, string sort, string search)
        {
            int total = 0;
            List<RegisterCode> queryData = _iRegisterCodeBll.GetRegisterCodeLogins(Convert.ToInt32(id), page, rows,
                order, sort, search, ref total);
            var data = queryData.Select(s => new
            {
                Id = s.Id,
                Value = s.Value,
                LoginTime=s.LoginTime.ToString(),
                SerialPort=s.SerialPort,
                MemberName = s.MemberName,
                MerchantName = s.MerchantName
            });

            return Json(new datagrid
            {
                total = total,
                rows = data
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>   
        [HttpPost]
        [SupportFilter]
        public ActionResult DeleteRegisterCodeLogin(FormCollection collection)
        {
            string returnValue = string.Empty;
            string[] deleteId = collection["query"].GetString().Split(',');
            if (deleteId != null && deleteId.Length > 0)
            {

                if (_iRegisterCodeBll.DeleteRegisterCodeLogin(ref validationErrors, Convert.ToInt32(deleteId[0])))
                {
                    LogClassModels.WriteServiceLog(Suggestion.DeleteSucceed + "，注册码绑定的Id为" +
                        string.Join(",", deleteId), "注册码绑定"
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
                    LogClassModels.WriteServiceLog(Suggestion.DeleteFail + "，注册码绑定的Id为" +
                        string.Join(",", deleteId) + "," + returnValue, "注册码绑定"
                        );//删除失败，写入日志
                }
            }

            return Json(returnValue);
        }
    }
}
