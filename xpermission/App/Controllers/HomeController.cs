using System.Web.Mvc;
using System.Collections.Generic;
using DAL;
using BLL;
using Common;
using App.Models;
using IBLL;
using Models;
namespace App.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Index()
        {
            Account account = GetCurrentAccount();
            if (account == null)
            {
                RedirectToAction("Index", "Account");
            }
            else
            {
                ViewData["PersonName"] = account.PersonName;
                IHomeBLL home = new HomeBLL();
                //在1.4版本中修改 
                ViewData["Menu"] = home.GetMenuByAccount(ref account);// 获取菜单
            }

            return View();
        }


        /// <summary>
        /// 根据父节点获取数据字典,绑定二级下拉框的时候使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetSysFieldByParent(string id, string parentid, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            ISysFieldHander baseDDL = new SysFieldHander();
            return Json(new SelectList(baseDDL.GetSysFieldByParent(id, parentid, value), "MyTexts", "MyTexts"), JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取列表中的按钮导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetToolbar(string id)
        {
            if (string.IsNullOrWhiteSpace(id) && id == "undefined")
            {
                return null;
            }
            Account account = GetCurrentAccount();
            if (account == null)
            {
                return Content(" <script type='text/javascript'> window.top.location='Account'; </script>");

            }
            ISysMenuSysRoleSysOperationBLL sro = new SysMenuSysRoleSysOperationBLL();
            List<SysOperation> sysOperations = sro.GetByRefSysMenuIdAndSysRoleId(id, account.RoleIds);
            List<toolbar> toolbars = new List<toolbar>();
            foreach (SysOperation item in sysOperations)
            {
                toolbars.Add(new toolbar() { handler = item.Function, iconCls = item.Iconic, text = item.Name });
            }
            return Json(toolbars, JsonRequestBehavior.AllowGet);
        }
    }
}


