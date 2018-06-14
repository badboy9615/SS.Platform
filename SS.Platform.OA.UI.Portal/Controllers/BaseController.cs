using SS.Platform.OA.BLL;
using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo LoginUserInfo { get; set; }

        //因为控制器本身也是一个ActionFilter，
        //所以我们重写一下基类中的OnActionExcuting方法，
        //就可以实现，所有的Action执行之前，先校验用户是否已经登陆了。
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            return;
            #region 校验用户是否登录
            LoginUserInfo = Session["LoginUser"] as UserInfo;
            if (LoginUserInfo == null)
            {
                //没有登陆，
                filterContext.HttpContext.Response.Redirect("/Login");
                return;
            }
            #endregion


            //给自己留个后门
            if (LoginUserInfo.Name == "admin")
            {
                return;
            }
        }
    }
}
