using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/
        public IBLL.IRoleService RoleService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllRoleInfos(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondName = SName ?? "";
            var roleList = RoleService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondName), d => d.Code,
                                               true);
            List<Role> lstDep = roleList as List<Role>;
            var data = new
            {
                total = total,
                rows = (from u in roleList
                        select
                            new { u.ID, u.Code, u.Name, u.SubTime, u.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(Role roleInfo)
        {
            if (roleInfo.Code == null)
            {
                return Content("请输入角色编码！");
            }
            if (roleInfo.Name == null)
            {
                return Content("请输入角色名称！");
            }
            //查找具有相同编码的角色，如果存在，不允许添加
            var roleList = RoleService.LoadEntities(u => u.Code == roleInfo.Code);
            if (roleList.Any())
            {
                return Content("角色编码不允许重复，请重新输入角色编码！");
            }
            roleInfo = initEntity(roleInfo);

            RoleService.Add(roleInfo);
            if (RoleService.SaveChanges() > 0)
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
            Role roleInfo = RoleService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = roleInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(Role role)
        {
            role.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            role.ModifiedTIme = DateTime.Now;
            if (RoleService.Update(role))
            {
                RoleService.SaveChanges();
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
                return Content("系统运行出错！");
            }

            string[] idStrs = ids.Split(',');
            List<int> idDelete = new List<int>();
            foreach (var idStr in idStrs)
            {
                int deleteId = int.Parse(idStr);
                idDelete.Add(deleteId);
            }

            if (Delete(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出错！");

        }

        public int Delete(params int[] ids)
        {
            foreach (var id in ids)
            {
                var roleInfo = RoleService.LoadEntities(u => u.ID == id).FirstOrDefault();
                roleInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return RoleService.SaveChanges();
        }
        #endregion
        public ActionResult ShowError()
        {
            throw new Exception("系统运行出现错误！");
        }

        private Role initEntity(Role role)
        {
            role.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            role.SubTime = DateTime.Now;
            role.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            role.ModifiedTIme = DateTime.Now;
            role.TakeEffect = true;
            role.TakeEffectTime = DateTime.Now;
            role.LoseEffectTime = new DateTime(9999, 12, 31);
            role.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return role;
        }
    }
}
