using SS.Platform.Common;
using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class UserInfoController : BaseController
    {
        //
        // GET: /UserInfo/

        public IBLL.IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }


        #region 显示表格数据
        public ActionResult GetAllUser(string SName)
        {
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var strCondition = SName ?? "";
            var userList = UserInfoService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondition), d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in userList
                        select
                            new { u.ID, u.Code, u.Name, u.Mail, u.Phone, u.QQNum, u.Remark }).ToList()
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
        public ActionResult AddSave(UserInfo userInfo)
        {
            //处理家政员基本信息保存
            if (userInfo.Code == null)
            {
                return Content("请输入登录名！");
            }
            if (userInfo.Name == null)
            {
                return Content("请输入显示名！");
            }

            var userList = UserInfoService.LoadEntities(u => u.Code == userInfo.Code);
            if (userList.Any())
            {
                return Content("登陆名在系统中已存在！");
            }
            userInfo = initEntity(userInfo);

            UserInfoService.Add(userInfo);
            if (UserInfoService.SaveChanges() > 0)
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

        public ActionResult GetUserInfoByID(string id)
        {
            var uid = int.Parse(id);
            UserInfo userInfo = UserInfoService.LoadEntities(u => u.ID == uid).FirstOrDefault();

            var result = new { model = userInfo };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 修改保存
        public ActionResult EditSave(UserInfo userInfo)
        {
            //处理用户信息保存
            if (userInfo.Name == null)
            {
                return Content("请输入显示名！");
            }

            userInfo.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            userInfo.ModifiedTIme = DateTime.Now;
            if (!userInfo.TakeEffect)
            {
                userInfo.LoseEffectTime = DateTime.Now;
            }
            else
            {
                userInfo.LoseEffectTime = new DateTime(9999, 12, 31);
            }
            UserInfoService.Update(userInfo);
            if (UserInfoService.SaveChanges() > 0)
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

            if (UserInfoService.DeleteIds(idDelete.ToArray()) > 0)
            {
                return Content("ok");
            }
            return Content("系统运行出现错误！");

        }
        #endregion


        #region 实体初始化

        private UserInfo initEntity(UserInfo userInfo)
        {
            userInfo.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            userInfo.SubTime = DateTime.Now;
            userInfo.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            userInfo.ModifiedTIme = DateTime.Now;
            //userInfo.TakeEffect = true;
            //userInfo.TakeEffectTime = DateTime.Now;
            //userInfo.LoseEffectTime = new DateTime(9999, 12, 31);
            userInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            userInfo.Pwd = CommonHelper.GetCipherText("123456"); ;
            return userInfo;
        }
        #endregion
    }
}
