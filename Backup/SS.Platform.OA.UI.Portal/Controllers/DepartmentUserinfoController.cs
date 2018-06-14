using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class DepartmentUserinfoController : BaseController
    {
        //
        // GET: /DepartmentUserinfo/
        public IBLL.IDepartmentService DepartmentService { get; set; }
        public IBLL.IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 加载部门树
        public ActionResult GetTreeDepNodes()
        {
            short del = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            var allDep = from d in DepartmentService.LoadEntities(u => u.DelFlag == del && u.TakeEffect)
                         select new { CategoryId = d.ID, ParentId = d.ParDept, Name = d.Name };

            return Json(allDep, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取部门用户数据
        public ActionResult GetDepartmentUser(int SDep)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var users = UserInfoService.LoadEntities(u =>u.Department!=null && u.Department.ID == SDep && u.DelFlag == delNormal);

            var data = new
            {
                Rows = (from u in users
                        select
                            new { u.ID, u.Code, u.Name, u.SubTime, u.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 获取所有用户数据
        public ActionResult GetAllUserInfo()
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var userInfoList = UserInfoService.LoadEntities(u => u.DelFlag == delNormal);
            var data = new
            {
                Total = 1,
                Rows = (from u in userInfoList
                        select
                            new { u.ID, u.Code, u.Name, u.SubTime, u.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加部门用户
        public ActionResult AddUser(string DepId, string UsersId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            int idDept = int.Parse(DepId);
            string[] idStrs = UsersId.Split(',');
            Department deptInfo = DepartmentService.LoadEntities(u => u.ID == idDept && u.DelFlag == delNormal).FirstOrDefault();
            if (deptInfo == null)
            {
                return Content("系统运行出错,错误信息为：部门为空！");
            }
            foreach (var idStr in idStrs)
            {
                int userId = int.Parse(idStr);
                UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == userId && u.DelFlag == delNormal).FirstOrDefault();
                //deptInfo.UserInfo.Add(userInfo);
                userInfo.Department = deptInfo;
            }
            if (UserInfoService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("系统运行出错！错误信息为：保存出错。");
            }
        }
        #endregion

        #region 删除部门用户
        public ActionResult DelUser(string DepId, string UsersId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            int idDept = int.Parse(DepId);
            string[] idStrs = UsersId.Split(',');
            Department deptInfo = DepartmentService.LoadEntities(u => u.ID == idDept && u.DelFlag == delNormal).FirstOrDefault();
            if (deptInfo == null)
            {
                return Content("系统运行出错！错误信息为：部门为空。");
            }
            foreach (var idStr in idStrs)
            {
                int userId = int.Parse(idStr);
                UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == userId && u.DelFlag == delNormal).FirstOrDefault();

                userInfo.Department = null;
            }
            if (UserInfoService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("系统运行出错！错误信息为：保存出错。");
            }
        }
        #endregion
    }
}
