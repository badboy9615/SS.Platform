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
        public IBLL.IRoleService RoleService { get; set; }
        //public IBLL.IActionInfoService ActionInfoService { get; set; }
        //public IBLL.IR_User_ActionInfoService R_User_ActionInfoService { get; set; }
        //public IBLL.IMenuInfoService MenuInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMenu()
        {
            short del = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;

            //返回当前用户的菜单的数据

            //加载用户的菜单数据：
            //加载用户所有的权限
            //加载用户所有的角色
            //加载用户所有的特殊权限

            //加载用户所有的角色,拿到当前用户
            //this.LoginUserInfo
            //var user = UserInfoService.LoadEntities(u => u.ID == this.LoginUserInfo.ID).FirstOrDefault();

            ////拿到了角色对应的权限的id
            //var allRoleActionIds = (from r in user.Role
            //                        from a in r.ActionInfo
            //                        where a.DelFlag == del && r.DelFlag == del
            //                        select a.ID).ToList();

            ////拿到了当前用户所有的允许的特殊权限
            //var allUserActionIsPass = (from r in user.R_User_ActionInfo
            //                           where r.Ispass == true
            //                           select r.ActionInfoID).ToList();

            ////合并起来的所有的都被允许的。
            //allRoleActionIds.AddRange(allUserActionIsPass);

            ////去掉不允许的
            //var allNotPassUserActions = (from r in user.R_User_ActionInfo
            //                             where r.Ispass == false
            //                             select r.ActionInfoID).ToList();
            //var result = (from a in allRoleActionIds
            //              where !allNotPassUserActions.Contains(a)
            //              select a).ToList();
            ////拿到当前用户所有的权限Id
            //result = result.Distinct().ToList();

            ////跟Menu表进行join查询就行了
            ////将菜单数据放到缓存里去
            //List<SS.Platform.OA.Model.MenuInfo> allMenus = Common.CacheHelper.Get<List<SS.Platform.OA.Model.MenuInfo>>("MenuInfo");
            //if (allMenus == null)
            //{
            //    allMenus = MenuInfoService.LoadEntities(m => true).ToList();
            //    Common.CacheHelper.Insert("MenuInfo", allMenus);
            //}
            ////var allMenus = MenuInfoService.LoadEntities(m => true);


            //var allActions = ActionInfoService.LoadEntities(a => true);

            //var allMenuData = from m in allMenus
            //                  from a in allActions
            //                  where result.Contains(m.ActionInfoId)
            //                  where a.ID == m.ActionInfoId
            //                  select new { icon = m.IconUrl, title = m.MenuName, url = a.Url };
            //return Json(allMenuData.ToList(), JsonRequestBehavior.AllowGet);

            ////{ icon: '../../Content/Images/Alien Folder.png', title: '树', url: '../tree/draggable.htm' },
            return Content("ok");
        }

        public ActionResult GetCurrentUser()
        {
            UserInfo loginUserInfo = Session["LoginUser"] as UserInfo;
            var result = new { model = loginUserInfo };
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
