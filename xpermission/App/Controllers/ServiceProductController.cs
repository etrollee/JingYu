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
    public class ServiceProductController : BaseController
    {
        IServiceProductBLL _iServiceProductBll;
        IMerchantBLL _iMerchantBll;
        ValidationErrors validationErrors = new ValidationErrors();

        public ServiceProductController()
            : this(new ServiceProductBLL(), new SysPersonBLL(),new MerchantBLL())
        { 
        }

        public ServiceProductController(ServiceProductBLL serviceProductBll, SysPersonBLL sysPersonBll, 
            MerchantBLL merchantBll)
        {
            _iServiceProductBll = serviceProductBll;
            _iMerchantBll = merchantBll;
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
            List<ServiceProduct> queryData = null;
            string syspersonId = GetCurrentAccount().Id;
            queryData = _iServiceProductBll.GetByParam(syspersonId,Convert.ToInt32(id), page, rows, 
                order, sort, search, ref total);

            return Json(new datagrid
            {
                total = total,
                rows = queryData.Select(s => new 
                {
                    Id=s.Id,
                    MerchantId=s.Merchant.Name,
                    Name=s.Name,
                    Description=s.Description,
                    Star=s.Star,
                    Worth=s.Worth
                })
            });
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var item = _iServiceProductBll.GetById(id);
            return View(item);
        }

        /// <summary>
        /// 首次创建
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(string id)
        {
            string syspersonId = GetCurrentAccount().Id;
            var merchant= _iMerchantBll.GetRefSysPerson(syspersonId);
            ViewBag.Merchant = merchant;
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="serviceProduct"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public ActionResult Create(ServiceProduct serviceProduct)
        {

            if (serviceProduct!=null)
            {
                string returnValue = string.Empty;
                if (string.IsNullOrWhiteSpace(serviceProduct.MerchantId))
                {
                    serviceProduct.Worth = 0;
                    string syspersonId = GetCurrentAccount().Id;
                    serviceProduct.MerchantId = _iMerchantBll.GetRefSysPerson(syspersonId).Id;
                }
                if (_iServiceProductBll.Create(ref validationErrors,serviceProduct))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertException+",服务产品的信息的Id为"+serviceProduct.Id,"服务产品");
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
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，服务产品的信息，" + returnValue, "服务产品"
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
        public ActionResult Edit(int id)
        {
            ServiceProduct entity = _iServiceProductBll.GetById(id);
            string syspersonId = GetCurrentAccount().Id;
            var merchant = _iMerchantBll.GetRefSysPerson(syspersonId);
            ViewBag.Merchant = merchant;
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
        public ActionResult Edit(string id, ServiceProduct entity)
        {

            if (entity != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                if (_iServiceProductBll.Edit(ref validationErrors, entity))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，服务产品信息的Id为" + id, "服务产品"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，服务产品信息的Id为" + id + "," + returnValue,
                        "服务产品");//写入日志                           
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

                if (_iServiceProductBll.DeleteCollection(ref validationErrors, deleteId))
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
