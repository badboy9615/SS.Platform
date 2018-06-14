using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class ModuleController : BaseController
    {
        //
        // GET: /Module/
        public IBLL.IModuleService ModuleService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllModule(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var strCondition = SName ?? "";
            var moduleList = ModuleService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondition), d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in moduleList
                        select
                            new { u.ID, u.Code, u.Name, u.Url, u.Remark, u.SubBy, u.SubTime }).ToList()
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
        public ActionResult AddSave(Module module)
        {
            //处理模块信息保存
            if (module.Name == null)
            {
                return Content("请输入模块名！");
            }

            var moduleList = ModuleService.LoadEntities(u => u.Name == module.Name);
            if (moduleList.Any())
            {
                return Content("模块在系统中已存在！");
            }
            //查找已有模块
            var moduList = ModuleService.LoadEntities(u => 1 == 1);

            string moduCode;
            var moduCodeList = (from u in moduList
                                orderby u.Code descending
                                select
                                    new { u.Code });
            if (moduCodeList.Any())
            {
                moduCode = moduCodeList.Take(1).ToList()[0].Code;
                int intLs = int.Parse(moduCode);
                intLs++;
                module.Code = intLs.ToString();
            }
            else
            {
                module.Code = "1001";
            }

            module = initEntity(module);

            ModuleService.Add(module);
            if (ModuleService.SaveChanges() > 0)
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

        public ActionResult GetModuleByID(string id)
        {
            var mid = int.Parse(id);
            Module module = ModuleService.LoadEntities(u => u.ID == mid).FirstOrDefault();

            var result = new { model = module };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 修改保存
        public ActionResult EditSave(Module module)
        {
            if (module.Name == null)
            {
                return Content("请输入模块名称！");
            }

            module.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            module.ModifiedTIme = DateTime.Now;
            module.TakeEffect = true;
            module.LoseEffectTime = DateTime.Parse("9999-12-31");

            ModuleService.Update(module);
            if (ModuleService.SaveChanges() > 0)
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

            if (ModuleService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出现错误！");

        }
        #endregion

        #region 实体初始化

        private Module initEntity(Module module)
        {
            module.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            module.SubTime = DateTime.Now;
            module.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            module.ModifiedTIme = DateTime.Now;
            module.TakeEffect = true;
            module.TakeEffectTime = DateTime.Now;
            module.LoseEffectTime = new DateTime(9999, 12, 31);
            module.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return module;
        }
        #endregion

    }
}
