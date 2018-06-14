using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Platform.OA.BLL;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    //public class HomeController: Controller
    public class HomeController: BaseController
    {
        public IBLL.IUserInfoService UserInfoService { get; set; }
        //public IBLL.IRoleService RoleService { get; set; }
        //public IBLL.IActionInfoService ActionInfoService { get; set; }
        //public IBLL.IR_User_ActionInfoService R_User_ActionInfoService { get; set; }
        //public IBLL.IMenuInfoService MenuInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMenu()
        {
            return View();
        }

        public ActionResult GetCurrentUser()
        {
            var result = new { model = this.LoginUserInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();//没有登陆，
            return Content("ok");
        }
    }
}
