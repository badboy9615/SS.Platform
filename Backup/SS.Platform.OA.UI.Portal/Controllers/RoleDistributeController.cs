using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class RoleDistributeController : Controller
    {
        //
        // GET: /RoleDistribute/
        public IBLL.IDepartmentService DepartmentService { get; set; }
        public IBLL.IUserInfoService UserInfoService { get; set; }
        public IBLL.IRoleService RoleService { get; set; }

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

        #region 获取所有用户数据
        public ActionResult GetAllUserInfo()
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var userInfoList = UserInfoService.LoadEntities(u => u.DelFlag == delNormal);
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

        #region 获取部门角色数据
        public ActionResult GetDepartmentRole(int SDep)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            Department deptInfo = DepartmentService.LoadEntities(u => u.ID == SDep && u.DelFlag == delNormal).FirstOrDefault();
            var roleList = RoleService.LoadEntities(u => u.DelFlag == delNormal).ToList();



            if (deptInfo != null)
            {
                var data = new
                {
                    Rows = (from u in deptInfo.Role
                            select
                                new { id = u.ID, text = u.Name }).ToList()
                   ,
                    RowsExcept = (from u in roleList.Except(deptInfo.Role)
                                  select
                                      new { id = u.ID, text = u.Name }).ToList()
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("选择的部门不存在！");
            }

            #endregion
        }
        #endregion

        #region 获取用户角色数据
        public ActionResult GetUserRole(int SUser)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == SUser && u.DelFlag == delNormal).FirstOrDefault();
            var roleList = RoleService.LoadEntities(u => u.DelFlag == delNormal).ToList();

            if (userInfo != null)
            {
                var data = new
                {
                    Rows = (from u in userInfo.Role
                            select
                                new { id = u.ID, text = u.Name }).ToList()
                   ,
                    RowsExcept = (from u in roleList.Except(userInfo.Role)
                                  select
                                      new { id = u.ID, text = u.Name }).ToList()
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("选择的用户不存在！");
            }

            #endregion
        }
        #endregion

        #region 部门角色保存
        public ActionResult DeptRoleSave(string DepId, string RolesId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            int idDept = int.Parse(DepId);
            string[] idStrs = null;
            //用户选择的权限
            List<Role> deptRoleSelList = new List<Role>();
            if (RolesId != "")
            {
                idStrs = RolesId.Split(',');
            }
            Department deptInfo = DepartmentService.LoadEntities(u => u.ID == idDept && u.DelFlag == delNormal).FirstOrDefault();
            if (deptInfo == null)
            {
                return Content("系统运行出错！错误信息为：部门为空。");
            }
            //给部门添加用户选择的角色
            if (idStrs != null)
            {
                //添加已选
                foreach (var idStr in idStrs)
                {
                    int roleId = int.Parse(idStr);
                    Role roleInfo = RoleService.LoadEntities(u => u.ID == roleId && u.DelFlag == delNormal).FirstOrDefault();
                    deptRoleSelList.Add(roleInfo);
                    //只添加原来没有的角色
                    if (!deptInfo.Role.Contains(roleInfo))
                    {
                        deptInfo.Role.Add(roleInfo);
                    }
                }
                //已选权限
                var deptRoleDelList = deptInfo.Role.Except(deptRoleSelList).ToList();
                foreach (var roleEx in deptRoleDelList)
                {
                    deptInfo.Role.Remove(roleEx);
                }
            }
            else
            {
                deptInfo.Role.Clear();
            }
            if (DepartmentService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("ok");
            }
        }
        #endregion

        #region 用户角色保存
        public ActionResult UserRoleSave(string UserId, string RolesId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            int idUser = int.Parse(UserId);
            string[] idStrs = null;
            //用户选择的权限
            List<Role> userRoleSelList = new List<Role>();
            if (RolesId != "")
            {
                idStrs = RolesId.Split(',');
            }
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == idUser && u.DelFlag == delNormal).FirstOrDefault();
            if (userInfo == null)
            {
                return Content("系统运行出错！错误信息为：用户为空。");
            }
            //给用户添加用户选择的角色
            if (idStrs != null)
            {
                //添加已选
                foreach (var idStr in idStrs)
                {
                    int roleId = int.Parse(idStr);
                    Role roleInfo = RoleService.LoadEntities(u => u.ID == roleId && u.DelFlag == delNormal).FirstOrDefault();
                    userRoleSelList.Add(roleInfo);
                    //只添加原来没有的角色
                    if (!userInfo.Role.Contains(roleInfo))
                    {
                        userInfo.Role.Add(roleInfo);
                    }
                }
                //已选权限
                var userRoleDelList = userInfo.Role.Except(userRoleSelList).ToList();
                foreach (var roleEx in userRoleDelList)
                {
                    userInfo.Role.Remove(roleEx);
                }
            }
            else
            {
                userInfo.Role.Clear();
            }
            if (UserInfoService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("ok");
            }
        }
        #endregion
    }
}
