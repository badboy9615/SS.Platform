using SS.Platform.OA.IBLL;
using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class R_User_ActionInfoController : BaseController
    {
        //
        // GET: /R_User_ActionInfo/
        public IR_User_ActionInfoService R_User_ActionInfoService { get; set; }
        public IUserInfoService UserInfoService { get; set; }
        public IActionInfoService ActionInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 获取所有用户数据
        public ActionResult GetAllUserInfo()
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var userInfoList = UserInfoService.LoadEntities(u => u.DelFlag == delNormal).ToList();
            var data = new
            {
                Rows = (from u in userInfoList
                        select
                            new { u.ID, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 获取用户权限数据
        public ActionResult GetUserAction(int SUser)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            var userActionList = R_User_ActionInfoService.LoadEntities(u => u.UserInfoID == SUser && u.DelFlag == delNormal).ToList();
            
            var data=new
            {
                Rows= (from u in userActionList 
                        select 
                        new{ID=u.ID,Action=u.ActionInfo.Name,u.IsPass}).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取所有权限数据
        public ActionResult GetAllActionInfo()
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var actionInfoList = ActionInfoService.LoadEntities(u => u.DelFlag == delNormal);
            var data = new
            {
                Total = 1,
                Rows = (from u in actionInfoList
                        select
                            new { u.ID, u.Code, u.Name}).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(R_User_ActionInfo act)
        {
            if (act.UserInfoID == 0)
            {
                return Content("请选择用户");
            }
            if (act.ActionInfoID == 0)
            {
                return Content("请选择权限");
            }
            //查找已有权限
            var userAction = R_User_ActionInfoService.LoadEntities(u => u.UserInfoID == act.UserInfoID && u.ActionInfoID==act.ActionInfoID).FirstOrDefault();
            if (userAction!=null)
            {
                if (userAction.DelFlag==(short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted)
                {
                    userAction.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
                    if (R_User_ActionInfoService.Update(userAction))
                    {
                        R_User_ActionInfoService.SaveChanges();
                        return Content("ok");
                    }
                }
                else
                {
                    return Content("该用户权限已经存在，不允许重复添加！");
                }
            }
            //查找用户编码
            var user = UserInfoService.LoadEntities(u => u.ID == act.UserInfoID).FirstOrDefault();
            //查找权限编码
            var action = ActionInfoService.LoadEntities(u => u.ID == act.ActionInfoID).FirstOrDefault();
            //#region 生成编码
            act.Code=user.Code+"-"+action.Code;
            act.Name=user.Name+"-"+action.Name;
            act.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;

            R_User_ActionInfoService.Add(act);
            if (R_User_ActionInfoService.SaveChanges() > 0)
            {
                return Content("ok");
            }

            return Content("添加失败了");
        }
        #endregion

        #region 修改
        public ActionResult Edit(string ids)
        {
            var id = int.Parse(ids);
            R_User_ActionInfo act = R_User_ActionInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = act };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(R_User_ActionInfo act)
        {
            if (R_User_ActionInfoService.Update(act))
            {
                R_User_ActionInfoService.SaveChanges();
                return Content("ok");
            }
            return Content("修改失败了！");
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

            if (R_User_ActionInfoService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出现错误！");

        }
        #endregion
    }
}
