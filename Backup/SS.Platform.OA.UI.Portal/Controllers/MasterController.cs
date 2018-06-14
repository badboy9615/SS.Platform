using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Master/
        public IBLL.IMasterService MasterService { get; set; }
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

        #region 获取部门负责人数据
        public ActionResult GetDepartmentMaster(int SDep)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var master = MasterService.LoadEntities(u => u.Department != null && u.Department.ID == SDep && u.DelFlag == delNormal);
            //所有用户
            var user = UserInfoService.LoadEntities(u => u.DelFlag == delNormal);
            var data = new
            {
                Rows = (from u in master
                        join m in user on u.UserInfoID equals m.ID
                        select
                            new { m.ID,m.Code, m.Name}).ToList()
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

        #region 添加部门负责人
        public ActionResult AddMaster(string DepId, string UsersId)
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
                //先检查系统中是否已经有这条记录
                Master masterExit = MasterService.LoadEntities(u => u.UserInfo == userInfo && u.Department==deptInfo).FirstOrDefault();
                if (masterExit !=null)
                {
                    if (masterExit.DelFlag != delNormal)
                    {
                        masterExit.DelFlag = delNormal;
                        MasterService.Update(masterExit);
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                Master master = new Master();
                master.Department = deptInfo;
                master.UserInfo = userInfo;
                master.DelFlag = delNormal;
                master.Code = deptInfo.Code + "-" + userInfo.Code;
                master.Name = deptInfo.Name + "-" + userInfo.Name;
                MasterService.Add(master);

            }
            if (MasterService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("系统运行出错！错误信息为：保存出错。");
            }
        }
        #endregion

        #region 删除部门负责人
        public ActionResult DelMaster(string DepId, string UsersId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            short delDelete=(short)Model.Enum.DelFlagEnum.Deleted;
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
                Master masterExit = MasterService.LoadEntities(u => u.UserInfo == userInfo && u.Department == deptInfo).FirstOrDefault();
                if (masterExit!=null)
                {
                    masterExit.DelFlag = delDelete;
                    MasterService.Update(masterExit);
                }
            }
            if (MasterService.SaveChanges() > 0)
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
