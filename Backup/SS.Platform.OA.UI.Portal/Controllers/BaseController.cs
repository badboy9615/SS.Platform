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
            //return;
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


            //#region 过滤权限

            ////校验用户是否拥有访问此动作的权限
            //string str = filterContext.HttpContext.Request.RawUrl;//   /UserInfo/Index


            //string httpMethod = filterContext.HttpContext.Request.HttpMethod.ToLower();

            //ActionInfoService actionInfoService = new ActionInfoService();

            //var currentUrlAction =//拿到当前请求地址和Method对应的权限
            //actionInfoService.LoadEntities(a => a.Url == str && a.HttpMethod.ToLower() == httpMethod)
            //                 .FirstOrDefault();

            ////第一个：如果当前没有权限数据跟当前的url地址对应。
            //if (currentUrlAction == null)
            //{
            //    Common.LogHelper.WriteLog(string.Format("用户：{0}在 时间：{1} 请求 {2} 请求类型：{3} 出现了没有权限的问题。 对方的IP是{4}", LoginUserInfo.ID, DateTime.Now, str, httpMethod, filterContext.HttpContext.Request.UserHostAddress));
            //    filterContext.HttpContext.Response.Redirect("/Error.html");
            //}


            ////下面看当用户有没有跟当前权限关联到一块
            ////1 号线： 用户特殊权限

            //short delNormal = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;

            //R_User_ActionInfoService rUserActionInfoService = new R_User_ActionInfoService();
            //var tempUserAction = (from a in rUserActionInfoService.LoadEntities(u => u.DelFlag == delNormal)
            //                      where a.ActionInfoID == currentUrlAction.ID && a.UserInfoID == LoginUserInfo.ID
            //                      select a).FirstOrDefault();
            //if (tempUserAction != null)
            //{
            //    if (tempUserAction.Ispass)
            //    {
            //        return;//直接允许请求了
            //    }
            //    else
            //    {
            //        Common.LogHelper.WriteLog(string.Format("用户：{0}在 时间：{1} 请求 {2} 请求类型：{3} 出现了没有权限的问题。 对方的IP是{4}", LoginUserInfo.ID, DateTime.Now, str, httpMethod, filterContext.HttpContext.Request.UserHostAddress));
            //        filterContext.HttpContext.Response.Redirect("/Error.html");
            //    }
            //}



            ////地铁2 号线和3号线
            ////首先拿到当前用户的所有的角色
            //IBLL.IUserInfoService userInfoService = new UserInfoService();
            //var user = userInfoService.LoadEntities(u => u.ID == LoginUserInfo.ID).FirstOrDefault();

            ////user.Role.ToList();
            //var tempRoleActions = (from r in user.Role
            //                       from a in r.ActionInfo
            //                       where a.ID == currentUrlAction.ID
            //                       select a).Count();

            //if (tempRoleActions <= 0)
            //{
            //    Common.LogHelper.WriteLog(string.Format("用户：{0}在 时间：{1} 请求 {2} 请求类型：{3} 出现了没有权限的问题。 对方的IP是{4}", LoginUserInfo.ID, DateTime.Now, str, httpMethod, filterContext.HttpContext.Request.UserHostAddress));
            //    filterContext.HttpContext.Response.Redirect("/Error.html");
            //}

            //#endregion
        }
    }
}
