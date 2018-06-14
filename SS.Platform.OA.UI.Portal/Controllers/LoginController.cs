using SS.Platform.Common;
using SS.Platform.OA.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckUser(string passWord, string userName, string authcode)
        {
            //第一教研验证码
            string sessionCode = Session["ValidateCode"] == null ? string.Empty : Session["ValidateCode"].ToString();

            if (userName == "")
            {
                return Content("请输入用户名！");
            }

            if (authcode == "")
            {
                return Content("请输入验证码！");
            }

            Session["ValidateCode"] = null;
            if (authcode.ToUpper() != sessionCode)
            {
                //ViewData["error"] = "<script> alert('验证码错误！')</script>";
                //ViewData["error"] = "验证码错误！";
                //return View();
                return Content("验证码错误！");
            }

            Model.UserInfo user =
                UserInfoService.LoadEntities(u => u.Code.ToLower() == userName.ToLower() && u.Pwd == CommonHelper.GetCipherText(passWord)).FirstOrDefault();

            if (user == null)
            {
                return Content("用户名或密码错误");
            }


            Session["LoginUser"] = user;

            return Content("ok");
        }

        public ActionResult VCode()
        {
            //引用的都是程序集。需要生成后才会有效果的。
            ValidateCodeHelper vCode = new ValidateCodeHelper();
            string code = vCode.CreateValidateCodeContainsLetter(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");

        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("CheckUser");
        }
    }
}
