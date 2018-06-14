using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class ControlController : BaseController
    {
        //
        // GET: /Control/
        public IBLL.IControlService ControlService { get; set; }
        public IBLL.IModuleService ModuleService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllControls(string SName, string SModu)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strName = SName ?? "";
            string strModu = SModu ?? "";
            var controlList = ControlService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strName) && d.Module.Name.Contains(strModu), d => d.Code,
                                               true);
            var moduList = ModuleService.LoadEntities(d => 1 == 1);
            var data = new
            {
                total = total,
                rows = (from ct in controlList
                        join md in moduList on ct.ModuleID equals md.ID
                        orderby md.Code
                        select
                            new { ct.ID, module = md.Name, ct.Code, ct.Name, ct.Url, ct.DelFlag, ct.SubTime, ct.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 显示模块数据
        public ActionResult GetAllModule(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;

            #region 分页查询
            var moduleList = ModuleService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(SName), d => d.Code,
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

        #region 添加
        public ActionResult Add(Control ctl)
        {
            if (ctl.ModuleID == null)
            {
                return Content("请选择所属模块");
            }
            if (ctl.Name == "")
            {
                return Content("请输入控制器名称");
            }
            if (ctl.Url == "")
            {
                return Content("请输入URL");
            }
            #region 生成控制器编码
            //查找模块编码
            var module = ModuleService.LoadEntities(u => u.ID == ctl.ModuleID).FirstOrDefault();
            //查找同一模块的控制器
            var ctlList = ControlService.LoadEntities(u => u.ModuleID == ctl.ModuleID);

            string ctlCode;
            var ctlCodeList = (from u in ctlList
                               orderby u.Code descending
                               select
                                   new { u.Code });
            if (ctlCodeList.Any())
            {
                ctlCode = ctlCodeList.Take(1).ToList()[0].Code;
                string strLs = ctlCode.Substring(ctlCode.Length - 3, 3);
                int intLs = int.Parse(strLs);
                intLs++;
                string strNewLs = ctlCode.Substring(0, ctlCode.Length - 3) + intLs.ToString("D3");
                ctl.Code = strNewLs;
            }
            else
            {
                ctl.Code = module.Code + "001";
            }
            #endregion
            ctl = initEntity(ctl);
            //ctl.Init();
            //ctl.ID = Guid.NewGuid();
            ControlService.Add(ctl);
            if (ControlService.SaveChanges() > 0)
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
            Control ctl = ControlService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = ctl };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(Control ctl)
        {
            ctl.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            ctl.ModifiedTIme = DateTime.Now;
            if (ControlService.Update(ctl))
            {
                ControlService.SaveChanges();
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
                var ctlInfo = ControlService.LoadEntities(u => u.ID == id).FirstOrDefault();
                ctlInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return ControlService.SaveChanges();
        }
        #endregion

        private Control initEntity(Control ct)
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
