using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class ActionDistributeController : Controller
    {
        //
        // GET: /ActionDistribute/
        public IBLL.IRoleService RoleService { get; set; }
        public IBLL.IActionInfoService ActionInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 获取所有角色数据
        public ActionResult GetAllRoleInfo()
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var roleInfoList = RoleService.LoadEntities(u => u.DelFlag == delNormal);
            var data = new
            {
                Rows = (from u in roleInfoList
                        select
                            new { u.ID, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 获取角色权限数据
        public ActionResult GetRoleAction(int SRole)
        {
            //拿到前台发送来的是当前页面和页的大小
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 查询
            Role role = RoleService.LoadEntities(u => u.ID == SRole && u.DelFlag == delNormal).FirstOrDefault();
            var actionList = ActionInfoService.LoadEntities(u => u.DelFlag == delNormal).ToList();

            if (role != null)
            {
                var data = new
                {
                    Rows = (from u in role.ActionInfo
                            select
                                new { id = u.ID, u.Name }).ToList()
                   ,
                    RowsExcept = (from u in actionList.Except(role.ActionInfo)
                                  select
                                      new { id = u.ID, u.Name }).ToList()
                };

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("选择的角色不存在！");
            }

            #endregion
        }
        #endregion

        #region 角色权限保存
        public ActionResult RoleActionSave(string RoleId, string ActionsId)
        {
            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            int idRole = int.Parse(RoleId);
            string[] idStrs = null;
            //用户选择的权限
            List<ActionInfo> roleActionSelList = new List<ActionInfo>();
            if (ActionsId != "")
            {
                idStrs = ActionsId.Split(',');
            }
            Role role = RoleService.LoadEntities(u => u.ID == idRole && u.DelFlag == delNormal).FirstOrDefault();
            if (role == null)
            {
                return Content("系统运行出错！错误信息为：角色为空。");
            }
            //给角色添加用户选择的权限
            if (idStrs != null)
            {
                //添加已选
                foreach (var idStr in idStrs)
                {
                    int actionId = int.Parse(idStr);
                    ActionInfo actionInfo = ActionInfoService.LoadEntities(u => u.ID == actionId && u.DelFlag == delNormal).FirstOrDefault();
                    roleActionSelList.Add(actionInfo);
                    //只添加原来没有的权限
                    if (!role.ActionInfo.Contains(actionInfo))
                    {
                        role.ActionInfo.Add(actionInfo);
                    }
                }
                //已选权限
                var roleActionDelList = role.ActionInfo.Except(roleActionSelList).ToList();
                foreach (var roleEx in roleActionDelList)
                {
                    role.ActionInfo.Remove(roleEx);
                }
            }
            else
            {
                role.ActionInfo.Clear();
            }
            if (RoleService.SaveChanges() > 0)
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
