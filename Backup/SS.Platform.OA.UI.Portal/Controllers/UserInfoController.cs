using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Platform.OA.Model;
using SS.Platform.Common;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class UserInfoController : BaseController
    {
        //
        // GET: /UserInfo/
        //IBLL.IUserInfoService UserInfoService = new UserInfoService();
        public IBLL.IUserInfoService UserInfoService { get; set; }
        public IBLL.IUserGroupService UserGroupService { get; set; }
        public IBLL.IRoleService RoleService { get; set; }
        public IBLL.IR_User_ActionInfoService R_User_ActionInfoService { get; set; }
        public IBLL.IActionInfoService ActionInfoService { get; set; }
        //IBLL.IR_User_ActionInfoService R_User_ActionInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllUserInfos(string SLoginName, string SShowName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondLoginName = SLoginName ?? "";
            string strCondShowName = SShowName ?? "";
            var userList = UserInfoService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Code.Contains(strCondLoginName) && d.Name.Contains(strCondShowName), d => d.UserGroupID,
                                               true);
            var userGroupList = UserGroupService.LoadEntities(d => 1 == 1);
            var data = new
            {
                total = total,
                rows = (from u in userList
                        join ug in userGroupList on u.UserGroupID equals ug.ID
                        orderby ug.Code, u.Code
                        select
                            new { u.ID, u.Code, u.Name, TakeEffect = u.TakeEffect == true ? "是" : "否", u.DelFlag, u.SubTime, u.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 显示用户组数据
        public ActionResult GetAllUserGroup(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;

            #region 分页查询
            var userGroupList = UserGroupService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(SName), d => d.Code,
                                               true);

            var data = new
            {
                total = total,
                rows = (from u in userGroupList
                        select
                            new { u.ID, u.Code, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(UserInfo userInfo)
        {
            var userList = UserInfoService.LoadEntities(u => u.Code == userInfo.Code);
            if (userList.Any())
            {
                return Content("登陆名在系统中已存在！");
            }
            userInfo = initEntity(userInfo);

            UserInfoService.Add(userInfo);
            if (UserInfoService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            return Content("添加失败了");
        }
        #endregion

        #region 批量删除
        public ActionResult DeleteIds(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Content("系统运行出现错误！");
            }

            //ids:  1,3,4
            string[] idStrs = ids.Split(',');
            List<int> idDelete = new List<int>();
            foreach (var idStr in idStrs)
            {
                int deleteId = int.Parse(idStr);
                idDelete.Add(deleteId);
            }

            if (UserInfoService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出现错误！");

        }
        #endregion

        #region 修改
        public ActionResult Edit(string ids)
        {
            var id = int.Parse(ids);
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            var result = new { model = userInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(UserInfo userInfo)
        {
            userInfo.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            userInfo.ModifiedTIme = DateTime.Now;
            if (UserInfoService.Update(userInfo))
            {
                UserInfoService.SaveChanges();
                return Content("ok");
            }

            return Content("修改失败了！");
        }

        #endregion

        #region 修改密码
        [HttpPost]
        public ActionResult EditPassword(string OldPass, string NewPass, string ConfPass)
        {
            LoginUserInfo = Session["LoginUser"] as UserInfo;
            if (LoginUserInfo.Pwd !=CommonHelper.GetCipherText(OldPass))
            {
                return Content("原密码输入错误！");
            }
            if (NewPass !=ConfPass)
            {
                return Content("新密码与确认密码不一致！");
            }
            UserInfo userInfopd = UserInfoService.LoadEntities(u => u.ID == LoginUserInfo.ID).FirstOrDefault();
            userInfopd.Pwd = CommonHelper.GetCipherText(NewPass);
            if (UserInfoService.Update(userInfopd))
            {
                UserInfoService.SaveChanges();
                return Content("ok");
            }
            else
            {
                return Content("密码修改失败！");
            }
        }

        #endregion


        //#region 设置角色
        //public ActionResult GetRoleInfo(Guid SUser)
        //{
        //    var delNormal = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
        //    UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == SUser && u.DelFlag == delNormal).FirstOrDefault();
        //    if (userInfo != null)
        //    {
        //        var allRoleInfo = RoleInfoService.LoadEntities(u => u.DelFlag == delNormal).ToList();
        //        var data = new
        //        {
        //            Rows = (from r in allRoleInfo
        //                    select
        //                        new { r.ID, r.Name, r.SubTime, r.SubBy, selected = userInfo.RoleInfo.Contains(r) }).ToList()
        //        };
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Content("选择的用户不存在！");
        //    }
        //}

        //[HttpPost]
        //public ActionResult SetUserRoleSave(string UserId, string RolesId)
        //{
        //    short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
        //    string[] idStrs = null;
        //    //用户选择的角色
        //    List<RoleInfo> userRoleSelList = new List<RoleInfo>();
        //    Guid idUser = Guid.Parse(UserId);
        //    if (RolesId != "")
        //    {
        //        idStrs = RolesId.Split(',');
        //    }

        //    UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == idUser && u.DelFlag == delNormal).FirstOrDefault();
        //    if (userInfo == null)
        //    {
        //        return Content("系统运行出错！");
        //    }
        //    /**************************************************************/
        //    userInfo.RoleInfo.Clear();
        //    if (idStrs != null)
        //    {
        //        foreach (var idStr in idStrs)
        //        {
        //            Guid roleId = Guid.Parse(idStr);
        //            RoleInfo roleInfo = RoleInfoService.LoadEntities(u => u.ID == roleId && u.DelFlag == delNormal).FirstOrDefault();
        //            userInfo.RoleInfo.Add(roleInfo);
        //        }
        //    }
        //    if (UserInfoService.Savechanges() > 0)
        //    {
        //        return Content("ok");
        //    }
        //    else
        //    {
        //        return Content("系统运行出错！");
        //    }
            /**************************************************************/
            //给用户添加用户选择的角色
            //if (idStrs != null)
            //{
            //    //添加已选
            //    foreach (var idStr in idStrs)
            //    {
            //        Guid roleId = Guid.Parse(idStr);
            //        RoleInfo roleInfo = RoleInfoService.LoadEntities(u => u.ID == roleId && u.DelFlag == delNormal).FirstOrDefault();
            //        userRoleSelList.Add(roleInfo);
            //        //只添加原来没有的角色
            //        if (!userInfo.RoleInfo.Contains(roleInfo))
            //        {
            //            userInfo.RoleInfo.Add(roleInfo);
            //        }
            //    }
            //    //已选权限
            //    var userRoleDelList = userInfo.RoleInfo.Except(userRoleSelList).ToList();
            //    foreach (var roleEx in userRoleDelList)
            //    {
            //        userInfo.RoleInfo.Remove(roleEx);
            //    }
            //}
            //else
            //{
            //    userInfo.RoleInfo.Clear();
            //}
            //if (UserInfoService.Savechanges() > 0)
            //{
            //    return Content("ok");
            //}
            //else
            //{
            //    return Content("ok");
            //}
        //}
        //#endregion


        #region 设置用户的特殊权限
        public ActionResult SetAction(int id)
        {
            short delNormal = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;

            ViewData.Model = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();


            ViewBag.ExistUserActions =
                R_User_ActionInfoService.LoadEntities(r => r.DelFlag == delNormal && r.UserInfoID == id).ToList();


            //后台往前天传递 所有的权限
            ViewBag.AllActionInfos = ActionInfoService.LoadEntities(a => a.DelFlag == delNormal).ToList();
            return View();
        }

        //添加特殊权限
        public ActionResult SetUserActionPasss(R_User_ActionInfo userAction)
        {
            var item =
              R_User_ActionInfoService.LoadEntities(r => r.UserInfoID == userAction.UserInfoID && r.ActionInfoID == userAction.ActionInfoID)
                                      .FirstOrDefault();

            if (item == null)//如果没有那么直接添加
            {
                R_User_ActionInfoService.Add(userAction);
                R_User_ActionInfoService.SaveChanges();
            }
            else//如果有那么直接修改
            {
                item.IsPass = userAction.IsPass;
                item.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
                R_User_ActionInfoService.SaveChanges();
            }
            return Content("ok");
        }

        //去除特殊权限
        public ActionResult RemoveUserAction(int UserInfoID, int ActionInfoID)
        {
            //R_User_ActionInfoService.Delete()   
            var item =
                R_User_ActionInfoService.LoadEntities(r => r.UserInfoID == UserInfoID && r.ActionInfoID == ActionInfoID)
                                        .FirstOrDefault();

            if (item != null)
            {
                item.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
                R_User_ActionInfoService.SaveChanges();
            }

            return Content("ok");

        }
        #endregion

        public ActionResult ShowError()
        {
            throw new Exception("系统运行出现错误！");
        }

        private UserInfo initEntity(UserInfo user)
        {
            user.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            user.SubTime = DateTime.Now;
            user.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            user.ModifiedTIme = DateTime.Now;
            user.TakeEffect = true;
            user.TakeEffectTime = DateTime.Now;
            user.LoseEffectTime = new DateTime(9999, 12, 31);
            user.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            user.Pwd = CommonHelper.GetCipherText("123456"); ;
            return user;
        }
    }
}
