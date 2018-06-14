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
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            string strCondName = SName ?? "";
            var moduleList = ModuleService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondName), d => d.Code,
                                               true);
            var data = new
            {
                total = total,
                rows = (from u in moduleList
                        select
                            new { u.ID, u.Code, u.Name, u.Url, u.SubTime, u.SubBy }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 添加
        public ActionResult Add(Module modu)
        {
            if (modu.Name == "")
            {
                return Content("请输入模块名称");
            }
            if (modu.Url == "")
            {
                return Content("请输入URL");
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
                modu.Code = intLs.ToString();
            }
            else
            {
                modu.Code = "1001";
            }
            modu = initEntity(modu);
            ModuleService.Add(modu);
            if (ModuleService.SaveChanges() > 0)
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
            Module moduInfo = ModuleService.LoadEntities(u => u.ID == id).FirstOrDefault();

            var result = new { model = moduInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(Module modu)
        {
            modu.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            modu.ModifiedTIme = DateTime.Now;
            if (ModuleService.Update(modu))
            {
                ModuleService.SaveChanges();
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
                var moduInfo = ModuleService.LoadEntities(u => u.ID == id).FirstOrDefault();
                moduInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return ModuleService.SaveChanges();
        }
        #endregion

        private Module initEntity(Module md)
        {
            md.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            md.SubTime = DateTime.Now;
            md.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            md.ModifiedTIme = DateTime.Now;
            md.TakeEffect = true;
            md.TakeEffectTime = DateTime.Now;
            md.LoseEffectTime = new DateTime(9999, 12, 31);
            md.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return md;
        }

    }
}
