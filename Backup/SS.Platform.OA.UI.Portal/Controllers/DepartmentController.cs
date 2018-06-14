using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class DepartmentController : BaseController
    {
        //
        // GET: /Department/
        public IBLL.IDepartmentService DepartmentService { get; set; }
        public IBLL.IUserInfoService UserInfoService { get; set; }
        public IBLL.IMasterService MasterService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllDepartmentInfos(string SName, string SDepMaster)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total,total1,total2 = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondName = SName == null ? "" : SName.Trim();
            string strCondDepMaster = SDepMaster == null ? "" : SDepMaster.Trim();
            
            //var depList =DepartmentService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal, d => d.Code, true);

            var depList =
                strCondDepMaster != "" ?
                DepartmentService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal
                    && d.Name.Contains(strCondName) && d.ParentDep.Name.Contains(strCondName), d => d.Code, true) : 
                DepartmentService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal
                && d.Name.Contains(strCondName)
                    , d => d.Code, true);
            var depMaster = MasterService.LoadPageEntities(pageSize, pageIndex, out total1, d => d.DelFlag == delNormal, d => d.Code, true);
            var userInfo = UserInfoService.LoadPageEntities(pageSize, pageIndex, out total2, d => d.DelFlag == delNormal, d => d.Code, true);
            var data = new
            {
                total = total,
                rows = (from u in depList
                        //join m in depMaster on u.ID equals m.DepartmentID
                        //join user in userInfo on m.UserInfoID equals user.ID
                        select
                            new { u.ID, u.Code, u.Name
                                //, DepMaster = user.Name
                                , u.DepLevel
                                , ParentDep =u.ParentDep==null? "": u.ParentDep.Name
                                , TakeEffect = u.TakeEffect == true ? "是" : "否", u.DelFlag, u.SubTime, u.SubBy 
                            }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 加载部门树
        public ActionResult GetTreeDepNodes()
        {
            short del = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            var allDep = from d in DepartmentService.LoadEntities(u => u.DelFlag == del && u.TakeEffect)
                         select new { CategoryId = d.ID, ParentId = d.ParDept, Name = d.Name };

            return Json(allDep, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 显示用户数据，以供选择负责人
        public ActionResult GetAllUsers(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 15 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            if (SName == null)
            {
                SName = "";
            }
            #region 分页查询
            var userList = UserInfoService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(SName) && d.TakeEffect, d => d.ID,
                                               true);

            var data = new
            {
                total = total,
                rows = (from u in userList
                        select
                            new { u.ID, u.Code, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(Department dept)
        {
            if (dept.Code == null)
            {
                //return Content("请输入部门编码");
            }
            if (dept.Name == null)
            {
                return Content("请输入部门名称");
            }
            //if (dept.Master == 0)
            //{
            //    return Content("请选择部门负责人");
            //}
            //顶级部门
            var deptListTop = DepartmentService.LoadEntities(u => u.ParDept == null);
            //上级部门
            var parentDept = DepartmentService.LoadEntities(u => u.ID == dept.ParDept).FirstOrDefault();
            //具有同一个上级部门的部门列表,即兄弟节点
            var deptListBr = DepartmentService.LoadEntities(u => u.ParDept == dept.ParDept);
            if (dept.ParDept == null)
            {
                dept.Code = "1";
                dept.DepLevel = 0;
                if (deptListTop.Any())
                {
                    return Content("系统中只允许一个顶级部门存在，请您选择上级部门！");
                }
            }
            else
            {
                string deptCode;
                var deptList = (from u in deptListBr
                                orderby u.Code descending
                                select
                                    new { u.Code });
                if (deptList.Any())
                {
                    deptCode = deptList.Take(1).ToList()[0].Code;
                    string strLs = deptCode.Substring(deptCode.Length - 3, 3);
                    int intLs = int.Parse(strLs);
                    intLs++;
                    string strNewLs = deptCode.Substring(0, deptCode.Length - 3) + intLs.ToString("D3");
                    dept.Code = strNewLs;
                }
                else
                {
                    dept.Code = parentDept.Code + "001";
                }
                dept.DepLevel = parentDept.DepLevel + 1;
            }
            dept = initEntity(dept);
            //dept.ID = Guid.NewGuid();
            DepartmentService.Add(dept);
            if (DepartmentService.SaveChanges() > 0)
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
            Department deptInfo = DepartmentService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = deptInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(Department dept)
        {
            dept.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            dept.ModifiedTIme = DateTime.Now;
            if (DepartmentService.Update(dept))
            {
                DepartmentService.SaveChanges();
                return Content("ok");
            }

            return Content("修改失败了！");
        }

        #endregion

        private Department initEntity(Department dept)
        {
            dept.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            dept.SubTime = DateTime.Now;
            dept.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            dept.ModifiedTIme = DateTime.Now;
            dept.TakeEffect = true;
            dept.TakeEffectTime = DateTime.Now;
            dept.LoseEffectTime = new DateTime(9999, 12, 31);
            dept.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return dept;
        }

    }
}
