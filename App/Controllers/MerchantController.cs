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
    public class MerchantController : BaseController
    {
        IMerchantBLL _merchantBll;
        IMerchantTypeBLL _merchantTypeBll;
        ISysPersonBLL _iSysPersonBll;
        ValidationErrors validationErrors = new ValidationErrors();
        public int editAction { get; set; }

        public MerchantController()
            : this(new MerchantBLL(), new MerchantTypeBLL(), new SysPersonBLL())
        {
        }

        public MerchantController(MerchantBLL merchantBll, MerchantTypeBLL merchantTypeBll, SysPersonBLL sysPersonBll)
        {
            _merchantBll = merchantBll;
            _merchantTypeBll = merchantTypeBll;
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
            string syspersonId = GetCurrentAccount().Id;
            var merchantId = _merchantBll.GetMerchantId(syspersonId);
            bool flag = false;
            if (!string.IsNullOrEmpty(merchantId))
            {
                flag = true;
            }
            List<Merchant> queryData = _merchantBll.GetByParam(merchantId, id, page, rows, order, sort, search, ref total);
            return Json(new datagrid
            {
                total = total,
                rows = queryData.Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    RegisterCode = flag?"":s.RegisterCode,
                    Contacts = flag ? "" : s.Contacts,
                    Telephone = flag ? "" : s.Telephone,
                    Cellphone = flag ? "" : s.Cellphone,
                    QQ = s.QQ,
                    SiteUrl = s.SiteUrl,
                    Address = s.Address,
                    ComprehensiveStar = flag ? 0: s.ComprehensiveStar,
                    Balance = flag ?null: s.Balance,
                    MerchantTypeId = flag ? "" : s.MerchantType.Name,
                    Description = flag ? "" : substring(filterRegex.Replace(s.Description ?? "", ""), 20),
                    SysPersonId = flag ? "" : s.SysPersonId,
                    IsVisible = flag ? "" : (s.IsVisible == 1 ? "是" : "否")
                })
            });
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Details(string id, bool getLogo = false)
        {
            Merchant merchant = _merchantBll.GetById(id);
            ViewBag.HasLogo = merchant.Logo != null;
            return View(merchant);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult Logo(string id)
        {
            var merchant = _merchantBll.GetById(id);
            if (merchant == null)
            {
                return File(new byte[0], "image/jpeg");
            }
            var hasLogo = merchant.Logo != null;
            return File(hasLogo ? merchant.Logo : new byte[0], "image/jpeg");
        }

        /// <summary>
        /// 首次创建
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult Create(string id)
        {
            ViewBag.MerchantTypeList = new SelectList(_merchantTypeBll.GetAll(), "Id", "Name");
            ViewBag.Merchants = new SelectList(_merchantBll.GetAll(), "Id", "Name");
            return View();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="merchant"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Merchant merchant, FormCollection collection)
        {
            if (merchant != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                merchant.Id = Result.GetNewId();
                merchant.MerchantTypeId = Convert.ToInt32(collection["MerchantTypeId"]);
                merchant.UpdateFlag = Guid.NewGuid();

                var file = Request.Files["image"];
                if (file != null)
                {
                    var len = (int)file.InputStream.Length;
                    var buffer = new byte[len];
                    file.InputStream.Read(buffer, 0, len);
                    merchant.Logo = buffer;
                }
                if (_merchantBll.Create(ref validationErrors, merchant))
                {
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed
                                        + "，商家的信息的Id为" + merchant.Id, "商家");//写入日志 
                    return Json(Suggestion.InsertSucceed);
                }
                else
                {
                    if (validationErrors != null && validationErrors.Count > 0)
                    {
                        validationErrors.ForEach(a =>
                        {
                            returnValue += a.ErrorMessage;
                        });
                    }
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，商家的信息，" + returnValue, "商家"
                        );//写入日志                      
                    return Json(Suggestion.InsertFail + "，" + returnValue); //提示插入失败
                }
            }
            return Json(Suggestion.InsertFail + "，请核对输入的数据的格式"); //提示输入的数据的格式不对 
        }

        /// <summary>
        /// 首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns> 
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.MerchantTypeList = new SelectList(_merchantTypeBll.GetAll(), "Id", "Name");
            string syspersonId = GetCurrentAccount().Id;
            var sysRole = _iSysPersonBll.GetRefSysRole(syspersonId).FirstOrDefault();
            Merchant m = null;
            if (sysRole.Power == 3)//商家
            {
                m = _iSysPersonBll.GetRefMerchant(syspersonId).FirstOrDefault();
            }
            Merchant merchant = null;
            if (m != null)
            {
                editAction = 1;
                ViewBag.Merchant = m;
                merchant = _merchantBll.GetById(m.Id);
                merchant.SysPersonId = syspersonId;
            }
            else
            {
                editAction = 0;
                ViewBag.Merchant = null;
                merchant = _merchantBll.GetById(id);
                merchant.OldRegisterCode = merchant.RegisterCode;
            }
            Session["editAction"] = editAction;

            ViewBag.HaveLogo = merchant.Logo != null;
            return View(merchant);
        }

        /// <summary>
        /// 提交编辑信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="collection">客户端传回的集合</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditMerchant(string id, Merchant merchant)
        {
            if (merchant != null && ModelState.IsValid)
            {
                string returnValue = string.Empty;
                // merchant.RegisterCode = merchant.OldRegisterCode;
                var file = Request.Files["image"];
                if (file != null)
                {
                    var len = (int)file.InputStream.Length;
                    var buffer = new byte[len];
                    file.InputStream.Read(buffer, 0, len);
                    merchant.Logo = buffer;
                }
                merchant.UpdateFlag = Guid.NewGuid();
                int action = Convert.ToInt32(Session["editAction"]);
                if (_merchantBll.Edit(ref validationErrors, merchant, action))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，商家信息的Id为" + id, "商家"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，商家信息的Id为" + id + "," + returnValue, "商家"
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

                if (_merchantBll.DeleteCollection(ref validationErrors, deleteId))
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

        /// <summary>
        /// 获取商家的关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RelationShip(string id, bool getMerchant = false)
        {
            if (!getMerchant)
            {
                ViewBag.Id = id;
                return View();
            }
            else
            {
                var merchants = _merchantBll.GetAll()
                                                        .Select(m => new
                                                        {
                                                            id = m.Id,
                                                            name = m.Name,
                                                            code = m.RegisterCode,// "123",
                                                            parentId = m.ParentId
                                                        })
                                                        .ToList();
                var self = merchants.Where(m => m.id == id).FirstOrDefault();
                var parent = merchants.Where(m => m.id == self.parentId).FirstOrDefault();
                var children = merchants.Where(m => m.parentId == self.id).ToList();
                var parentId = parent != null ? parent.id : "";
                var others = merchants.FindAll(m =>
                            {
                                return m.id != self.id
                                        && m.id != parentId
                                        && (children.Count > 0 ?
                                                children.All(c => { return c.id != m.id; })
                                                : true);
                            });
                var relationShip = new
                {
                    self = self,
                    parent = parent,
                    children = children,
                    others = others
                };
                return Json(relationShip, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 设置商家关系
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="oldParentId"></param>
        /// <param name="newParentId"></param>
        /// <param name="oldChildIds"></param>
        /// <param name="newChildIds"></param>
        /// <returns></returns>
        public JsonResult SetRelationShip(string merchantId, string oldParentId = null,
            string newParentId = null, string oldChildIds = null, string newChildIds = null)
        {
            var setSuccessed = _merchantBll.SetRelationShip(merchantId, oldParentId, newParentId,
                                    (oldChildIds ?? "").Split(new char[] { ',' })
                                    , (newChildIds ?? "").Split(new char[] { ',' }),
                                    ref validationErrors);
            var response = new
            {
                success = setSuccessed,
                msg = setSuccessed ? "商家关系设置成功。" : validationErrors.FirstOrDefault().ErrorMessage
            };
            return Json(response);
        }

        /// <summary>
        /// 修改商家资料
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeSelfInfo()
        {
            string syspersonId = GetCurrentAccount().Id;
            var m = _iSysPersonBll.GetRefMerchant(syspersonId).FirstOrDefault();
            Merchant merchant = _merchantBll.GetById(m.Id);
            ViewBag.HaveLogo = merchant.Logo != null;
            return View(merchant);
        }

        /// <summary>
        /// 提交修改信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="collection">客户端传回的集合</param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public ActionResult ChangeSelfInfo(string id, Merchant merchant)
        {
            if (merchant != null)
            {
                string returnValue = string.Empty;
                var file = Request.Files["image"];
                if (file != null)
                {
                    var len = (int)file.InputStream.Length;
                    var buffer = new byte[len];
                    file.InputStream.Read(buffer, 0, len);
                    merchant.Logo = buffer;
                }
                if (_merchantBll.ChangeSelfInfo(ref validationErrors, merchant))
                {
                    LogClassModels.WriteServiceLog(Suggestion.UpdateSucceed + "，商家信息的Id为" + id, "商家"
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
                    LogClassModels.WriteServiceLog(Suggestion.UpdateFail + "，商家信息的Id为" + id + "," + returnValue, "商家"
                        );//写入日志                           
                    return Json(Suggestion.UpdateFail + returnValue); //提示更新失败
                }
            }
            return Json(Suggestion.UpdateFail + "请核对输入的数据的格式"); //提示输入的数据的格式不对  
        }
    }
}
