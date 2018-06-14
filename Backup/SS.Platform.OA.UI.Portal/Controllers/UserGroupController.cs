using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class UserGroupController : BaseController
    {
        //
        // GET: /UserGroup/
        public IBLL.IUserGroupService UserGroupService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        #region 显示表格数据
        public ActionResult GetAllUserGroupInfos(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var strCondition = SName ?? "";
            var userGroupList = UserGroupService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondition), d => d.ID,
                                               true);
            var data = new
            {
                total = total,
                rows = (from u in userGroupList
                        select
                            new { u.ID, u.Code, u.Name,u.Remark}).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(UserGroup userGroup)
        {
            if (userGroup.Code == null)
            {
                return Content("请输入用户组编码！");
            }
            if (userGroup.Name == null)
            {
                return Content("请输入用户组名称！");
            }
            //查找具有相同编码的用户组，如果存在，不允许添加
            var ugList = UserGroupService.LoadEntities(u => u.Code == userGroup.Code);
            if (ugList.Any())
            {
                return Content("已存在相同编码的用户组，请重新输入用户组编码！");
            }
            userGroup = initEntity(userGroup);
            //userGroup.ID = Guid.NewGuid();

            UserGroupService.Add(userGroup);
            if (UserGroupService.SaveChanges() > 0)
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
            UserGroup userGroup = UserGroupService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = userGroup };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(UserGroup userGroup)
        {
            if (UserGroupService.Update(userGroup))
            {
                UserGroupService.SaveChanges();
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

            if (UserGroupService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出错！");

        }
        #endregion

        private UserGroup initEntity(UserGroup group)
        {
            //group.SubBy = 1;
            //group.SubTime = DateTime.Now;
            //group.ModifiedBy = 1;
            //group.ModifiedTIme = DateTime.Now;
            //group.TakeEffect = true;
            //group.TakeEffectTime = DateTime.Now;
            //group.LoseEffectTime = new DateTime(9999, 12, 31);
            group.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return group;
        }

    }
}
