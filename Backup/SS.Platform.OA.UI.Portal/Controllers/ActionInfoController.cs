using SS.Platform.OA.IBLL;
using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class ActionInfoController : BaseController
    {
        //
        // GET: /ActionInfo/

        public IBLL.IModuleService ModuleService { get; set; }
        public IBLL.IControlService ControlService { get; set; }
        public IBLL.IActionInfoService ActionInfoService { get; set; }


        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllActionInfos(string SModule, string SControl, string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondModule = SModule ?? "";
            string strCondControl = SControl ?? "";
            string strCondName = SName ?? "";
            var actionList = ActionInfoService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Control.Module.Name.Contains(strCondModule) && d.Control.Name.Contains(strCondControl) && d.Name.Contains(strCondName), d => d.Code,
                                               true);
            var data = new
            {
                total = total,
                rows = (from u in actionList
                        select
                            new { u.ID, Module = u.Control.Module.Name, Control = u.Control.Name, TakeEffect = u.TakeEffect == true ? "是" : "否", u.HttpMethod, u.Code, u.Name, u.ActionMethod, u.Url, u.SubBy, u.SubTime }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 根据控制器获得权限数据
        public ActionResult GetAllActionByControl(string SModule, string SControl, string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondControl = SControl ?? "";
            string strCondName = SName ?? "";
            var actionList = ActionInfoService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.ControlID == int.Parse(strCondControl) && d.Name.Contains(strCondName), d => d.Code,
                                               true);
            var data = new
            {
                total = total,
                rows = (from u in actionList
                        select
                            new { u.ID, Module = u.Control.Module.Name, Control = u.Control.Name,  u.Code, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 显示模块数据，以供选择模块
        public ActionResult GetAllModule(string SName)
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
            var moduleList = ModuleService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(SName) && d.TakeEffect, d => d.Code,
                                               true);

            var data = new
            {
                total = total,
                rows = (from u in moduleList
                        select
                            new { u.ID, u.Code, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 显示控制器数据，以供选择控制器
        public ActionResult GetAllControl(string SName, string SModuleID)
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
            if (string.IsNullOrEmpty(SModuleID))
            {
                return Content("请先选择所属模块！");
            }
            int guiModuleID = int.Parse(SModuleID);
            #region 分页查询
            var controlList = ControlService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(SName) && d.ModuleID == guiModuleID && d.TakeEffect, d => d.Code,
                                               true);

            var data = new
            {
                total = total,
                rows = (from u in controlList
                        select
                            new { u.ID, Module = u.Module.Name, u.Code, u.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(ActionInfo act)
        {
            //if (act.Control.Module == null)
            //{
            //    return Content("请选择模块");
            //}
            if (act.ControlID == 0)
            {
                return Content("请选择控制器");
            }
            if (act.Name == "")
            {
                return Content("请输入权限名称");
            }
            if (act.ActionMethod == "")
            {
                return Content("请输入方法名称");
            }
            //查找控制器编码
            var control = ControlService.LoadEntities(u => u.ID == act.ControlID).FirstOrDefault();
            #region 生成编码
            //查找同一控制器的权限
            var actList = ActionInfoService.LoadEntities(u => u.ControlID == act.ControlID);

            string actCode;
            //同一控制器下权限的编码
            var actCodeList = (from u in actList
                               orderby u.Code descending
                               select
                                   new { u.Code });
            if (actCodeList.Any())
            {
                actCode = actCodeList.Take(1).ToList()[0].Code;
                string strLs = actCode.Substring(actCode.Length - 3, 3);
                int intLs = int.Parse(strLs);
                intLs++;
                string strNewLs = actCode.Substring(0, actCode.Length - 3) + intLs.ToString("D3");
                act.Code = strNewLs;
            }
            else
            {
                act.Code = control.Code + "001";
            }
            #endregion
            #region 生成UrlD:\SS.Platform\SS.Platform.OA.Model\DataModel.edmx
            act.Url = "/" + control.Url + "/" + act.ActionMethod;
            #endregion
            act = initEntity(act);
            ActionInfoService.Add(act);
            if (ActionInfoService.SaveChanges() > 0)
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
            ActionInfo act = ActionInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = act };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(ActionInfo act)
        {
            act.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            act.ModifiedTIme = DateTime.Now;
            if (ActionInfoService.Update(act))
            {
                ActionInfoService.SaveChanges();
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
                var actInfo = ActionInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
                actInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return ActionInfoService.SaveChanges();
        }
        #endregion

        private ActionInfo initEntity(ActionInfo ct)
        {
            ct.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            ct.SubTime = DateTime.Now;
            ct.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            ct.ModifiedTIme = DateTime.Now;
            ct.TakeEffect = true;
            ct.TakeEffectTime = DateTime.Now;
            ct.LoseEffectTime = new DateTime(9999, 12, 31);
            ct.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return ct;
        }

    }
}
