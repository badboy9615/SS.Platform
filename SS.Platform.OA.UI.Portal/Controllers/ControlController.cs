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

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllControl(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var strCondition = SName ?? "";
            var controlList = ControlService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondition), d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in controlList
                        select
                            new { u.ID, u.Code, u.Name, u.Url, u.Remark, u.SubBy, u.SubTime,moduleName=u.Module.Name }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);

            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add()
        {
            return View();
        }

        #region 添加保存
        public ActionResult AddSave(Control control)
        {
            if (control.Name == null)
            {
                return Content("请输入控制器名！");
            }

            var controlList = ControlService.LoadEntities(u => u.Name == control.Name);
            if (controlList.Any())
            {
                return Content("控制器在系统中已存在！");
            }
            var contrList = ControlService.LoadEntities(u => 1 == 1);

            string contrCode;
            var contrCodeList = (from u in contrList
                                orderby u.Code descending
                                select
                                    new { u.Code });
            if (contrCodeList.Any())
            {
                contrCode = contrCodeList.Take(1).ToList()[0].Code;
                int intLs = int.Parse(contrCode);
                intLs++;
                control.Code = intLs.ToString();
            }
            else
            {
                control.Code = "1001";
            }

            control = initEntity(control);

            ControlService.Add(control);
            if (ControlService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            return Content("添加失败了");
        }
        #endregion

        #endregion

        #region 修改
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult GetControlByID(string id)
        {
            var cid = int.Parse(id);
            Control control = ControlService.LoadEntities(u => u.ID == cid).FirstOrDefault();

            var result = new { model = control };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 修改保存
        public ActionResult EditSave(Control control)
        {
            if (control.Name == null)
            {
                return Content("请输入控制器名称！");
            }

            control.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            control.ModifiedTIme = DateTime.Now;
            control.TakeEffect = true;
            control.LoseEffectTime = DateTime.Parse("9999-12-31");

            ControlService.Update(control);
            if (ControlService.SaveChanges() > 0)
            {
                return Content("ok");
            }
            return Content("修改失败了！");
        }
        #endregion

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

            if (ControlService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出现错误！");

        }
        #endregion

        #region 实体初始化

        private Control initEntity(Control control)
        {
            control.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            control.SubTime = DateTime.Now;
            control.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            control.ModifiedTIme = DateTime.Now;
            control.TakeEffect = true;
            control.TakeEffectTime = DateTime.Now;
            control.LoseEffectTime = new DateTime(9999, 12, 31);
            control.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return control;
        }
        #endregion

        #region 测试页面
        public ActionResult test()
        {
            return View();
        }
        #endregion

    }
}
