using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Models
{
    public class MyExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //处理错误消息。跳转到一个错误也页面
            ///using(Sql)
            //string strError = "["+DateTime.Now.ToString()+"]"+filterContext.Exception.ToString();
            Common.LogHelper.WriteLog(filterContext.Exception.ToString());

            //页面跳转到错误页面
            filterContext.HttpContext.Response.Redirect("/Error.html");

        }
    }
}