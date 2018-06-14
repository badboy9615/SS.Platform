using SS.Platform.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SS.Platform.OA.UI.Portal.Controllers
{
    public class EmployeeController : BaseController
    {
        //
        // GET: /Employee/;

        public IBLL.IEmployeeService EmployeeService { get; set; }
        public IBLL.IExperienceService ExperienceService { get; set; }
        public IBLL.IFamilyService FamilyService { get; set; }
        public IBLL.ITrainService TrainService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 显示表格数据
        public ActionResult GetAllEmployee(string SName)
        {
            //"[{ id: 1, name: \"林三\", sex: \"男\", birthday: \"1989/01/12\", score: 63.3 }]"
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            var strCondition = SName ?? "";
            var userGroupList = EmployeeService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.Name.Contains(strCondition), d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in userGroupList
                        select
                            new { u.ID, u.Code, u.Name,u.zp,u.xb,u.nl,u.wh,u.hyzk,u.sg,u.tz,u.lrsj,u.qwgz,u.gzzt,u.ModifiedTIme,u.qwjob,u.jtzz,u.myhlz,u.yyszgz, u.Remark }).ToList()
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
        public ActionResult AddSave(Employee employee)
        {
            //处理家政员基本信息保存
            if (employee.Name == null)
            {
                return Content("请输入姓名！");
            }
            if (employee.nl == 0)
            {
                return Content("请输入年龄！");
            }
            if (employee.xb == null)
            {
                return Content("请选择性别！");
            }
            employee = initEntity(employee);

            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase FileSfzt = files["sfzt"];
            string FileName = FileSfzt.FileName; //上传的原文件名  
            string guidSfzt = "";
            if (FileName != null && FileName != "")
            {
                string FileType = FileName.Substring(FileName.LastIndexOf(".") + 1); //得到文件的后缀名  
                guidSfzt = employee.Code+"-SFZ-"+System.Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名  
                FileSfzt.SaveAs(Server.MapPath("/Upload/") + guidSfzt); //保存操作  
            }
            employee.sfzt = guidSfzt;

            HttpPostedFileBase FileZp = files["Zp"];
            string FileNameZp = FileZp.FileName; //上传的原文件名  
            string guidZp = "";
            if (FileNameZp != null && FileNameZp != "")
            {
                string FileType = FileNameZp.Substring(FileNameZp.LastIndexOf(".") + 1); //得到文件的后缀名  
                guidZp = employee.Code + "-ZP-" + System.Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名  
                FileZp.SaveAs(Server.MapPath("/Upload/") + guidZp); //保存操作  
            }
            employee.zp = guidZp;

            string jtStr = Request.Form["jtStr"].ToString();
            string gzStr = Request.Form["gzStr"].ToString();
            string pxStr = Request.Form["pxStr"].ToString();

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Family> listJt = js.Deserialize<List<Family>>(jtStr);

            for (int i = 0; i < listJt.Count; i++)
            {
                Family family1 = initFamily(listJt[i]);
                family1.Employee = employee;
                family1.Code = employee.Code + "-F" + (i + 1).ToString();
                employee.Family.Add(family1);
            }

            List<Experience> listGz = js.Deserialize<List<Experience>>(gzStr);

            for (int i = 0; i < listGz.Count; i++)
            {
                Experience experience = initExperience(listGz[i]);
                experience.Employee = employee;
                experience.Code = employee.Code + "-E" + (i + 1).ToString();
                employee.Experience.Add(experience);
            }

            List<Train> listPx = js.Deserialize<List<Train>>(pxStr);

            //foreach (Train family in listPx)
            for (int i = 0; i < listPx.Count; i++)
            {
                Train train = initTrain(listPx[i]);
                train.Employee = employee;
                train.Code = employee.Code + "-T" + (i + 1).ToString();
                employee.Train.Add(train);
            }

            EmployeeService.Add(employee);

            if (EmployeeService.SaveChanges() > 0)
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

        public ActionResult GetEmployeeByID(string id)
        {
            var eid = int.Parse(id);
            Employee employee = EmployeeService.LoadEntities(u => u.ID == eid).FirstOrDefault();

            employee.Family.Clear();
            employee.Experience.Clear();
            employee.Train.Clear();

            var result = new { model = employee };
            JsonResult str = Json(result, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSave(Employee employee)
        {
            if (employee.Name == null)
            {
                return Content("请输入姓名！");
            }
            if (employee.nl == 0)
            {
                return Content("请输入年龄！");
            }
            if (employee.xb == null)
            {
                return Content("请选择性别！");
            }

            string strZp = Request.Form["strZp"].ToString();
            string strSfzt = Request.Form["strSfzt"].ToString();

            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase FileSfzt = files["sfzt"];
            string FileName = FileSfzt.FileName; //上传的原文件名 
            if (FileName != "")    //换了图片
            {
                string guidSfzt = "";
                if (FileName != null && FileName != "")
                {
                    string FileType = FileName.Substring(FileName.LastIndexOf(".") + 1); //得到文件的后缀名  
                    guidSfzt = employee.Code + "-SFZ-" + System.Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名  
                    FileSfzt.SaveAs(Server.MapPath("/Upload/") + guidSfzt); //保存操作  
                }
                employee.sfzt = guidSfzt;
            }
            else
            {
                employee.sfzt = strSfzt;
            }


            HttpPostedFileBase FileZp = files["Zp"];
            string FileNameZp = FileZp.FileName; //上传的原文件名  
            if (FileNameZp != "")    //换了图片
            {
                string guidZp = "";
                if (FileNameZp != null && FileNameZp != "")
                {
                    string FileType = FileNameZp.Substring(FileNameZp.LastIndexOf(".") + 1); //得到文件的后缀名  
                    guidZp = employee.Code + "-ZP-" + System.Guid.NewGuid().ToString() + "." + FileType; //得到重命名的文件名  
                    FileZp.SaveAs(Server.MapPath("/Upload/") + guidZp); //保存操作  
                }
                employee.zp = guidZp;
            }
            else
            {
                employee.zp = strZp;
            }

            string jtStr = Request.Form["jtStr"].ToString();
            string gzStr = Request.Form["gzStr"].ToString();
            string pxStr = Request.Form["pxStr"].ToString();

            JavaScriptSerializer js = new JavaScriptSerializer();
            //处理家庭成员
            int jtCount = FamilyService.GetRecordCoutn(u => u.EmployeeID == employee.ID);
            List<Family> listJt = js.Deserialize<List<Family>>(jtStr);
            for (int j = 0; j < listJt.Count; j++)
            {
                Family family1 = listJt[j];
                family1.Employee = employee;
                family1.EmployeeID = employee.ID;
                family1.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                family1.ModifiedTIme = DateTime.Now;
                if (family1.Code == null)
                {
                    family1 = initFamily(family1);
                    family1.Code = employee.Code + "-F" + (jtCount + 1).ToString();
                    FamilyService.Add(family1);
                }
                else
                {

                    family1.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                    family1.ModifiedTIme = DateTime.Now;
                    FamilyService.Update(family1);
                }
            }
            FamilyService.SaveChanges();

            //处理工作经历
            int gzCount = ExperienceService.GetRecordCoutn(u => u.EmployeeID == employee.ID); 
            List<Experience> listGz = js.Deserialize<List<Experience>>(gzStr);
            for (int i = 0; i < listGz.Count; i++)
            {
                Experience experience = listGz[i];
                experience.Employee = employee;
                experience.EmployeeID = employee.ID;
                experience.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                experience.ModifiedTIme = DateTime.Now;
                if (experience.Code == null)
                {
                    experience = initExperience(experience);
                    experience.Code = employee.Code + "-E" + (gzCount + 1).ToString();
                    ExperienceService.Add(experience);
                }
                else
                {
                    experience.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                    experience.ModifiedTIme = DateTime.Now;
                    ExperienceService.Update(experience);
                }
            }
            //处理培训经历
            int pxCount = TrainService.GetRecordCoutn(u => u.EmployeeID == employee.ID);
            List<Train> listPx = js.Deserialize<List<Train>>(pxStr);
            for (int i = 0; i < listPx.Count; i++)
            {
                Train train = listPx[i];
                train.Employee = employee;
                train.EmployeeID = employee.ID;
                train.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                train.ModifiedTIme = DateTime.Now;
                if (train.Code == null)
                {
                    train = initTrain(train);
                    train.Code = employee.Code + "-T" + (pxCount + 1).ToString();
                    TrainService.Add(train);
                }
                else
                {
                    train.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
                    train.ModifiedTIme = DateTime.Now;
                    TrainService.Update(train);
                }
            }

            employee.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            employee.ModifiedTIme = DateTime.Now;
            if (EmployeeService.Update(employee))
            {
                TrainService.SaveChanges();
                ExperienceService.SaveChanges();
                FamilyService.SaveChanges();
                EmployeeService.SaveChanges();
                return Content("ok");
            }

            return Content("修改失败了！");
        }
        #endregion

        #region 获取工作经历
        public ActionResult GetAllEpGzjl(string eid)
        {
            //"[{ id: 1, name: \"林三\", sex: \"男\", birthday: \"1989/01/12\", score: 63.3 }]"
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            int intEid;
            if (eid!="")
            {
                intEid = int.Parse(eid);
            }
            else
            {
                intEid = 0;
            }

            var gzjlList = ExperienceService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.EmployeeID == intEid, d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in gzjlList
                        select
                            new { u.ID, u.Code, u.Name
                                ,Qssj = u.Qssj.ToString("yyyy-MM-dd")
                                ,Jssj = u.Jssj.ToString("yyyy-MM-dd")
                                , u.Khxm, u.Gzdd, u.Gzpd, u.SubBy
                                ,SubTime= u.SubTime.ToString("yyyy-MM-dd")
                                ,TakeEffectTime= u.TakeEffectTime.ToString("yyyy-MM-dd")
                                ,LoseEffectTime= u.LoseEffectTime.ToString("yyyy-MM-dd")
                                , u.Remark }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);

            #endregion
        }
        #endregion

        #region 获取家庭成员
        public ActionResult GetAllEpFamily(string eid)
        {
            //"[{ id: 1, name: \"林三\", sex: \"男\", birthday: \"1989/01/12\", score: 63.3 }]"
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            int intEid;
            if (eid != "")
            {
                intEid = int.Parse(eid);
            }
            else
            {
                intEid = 0;
            }

            var jtcyList = FamilyService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.EmployeeID == intEid, d => d.ID,
                                               true);
            
            var data = new
            {
                Total = total,
                Rows = (from u in jtcyList
                        select
                            new { u.ID, u.Code, u.Name, u.Relation, u.Phone,u.SubBy
                                ,SubTime=u.SubTime.ToString("yyyy-MM-dd")
                                ,TakeEffectTime=u.TakeEffectTime.ToString("yyyy-MM-dd")
                                ,LoseEffectTime=u.LoseEffectTime.ToString("yyyy-MM-dd")
                                , u.Remark }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);

            #endregion
        }
        #endregion

        #region 获取培训经历
        public ActionResult GetAllEpTrain(string eid)
        {
            //"[{ id: 1, name: \"林三\", sex: \"男\", birthday: \"1989/01/12\", score: 63.3 }]"
            //拿到前台发送来的是当前页面和页的大小
            int pageSize = Request["pageSize"] == null ? 10 : int.Parse(Request["pageSize"]);
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int total = 0;

            short delNormal = (short)Model.Enum.DelFlagEnum.Normal;
            #region 分页查询
            int intEid;
            if (eid != "")
            {
                intEid = int.Parse(eid);
            }
            else
            {
                intEid = 0;
            }

            var pxjlList = TrainService.LoadPageEntities(pageSize, pageIndex, out total, d => d.DelFlag == delNormal && d.EmployeeID == intEid, d => d.ID,
                                               true);

            var data = new
            {
                Total = total,
                Rows = (from u in pxjlList
                        select
                            new { u.ID, u.Code, u.Name
                                , Pxsj = u.Pxsj.ToString("yyyy-MM-dd")
                                , u.Pxqx, u.Pxlx, u.SubBy
                                ,SubTime= u.SubTime.ToString("yyyy-MM-dd")
                                , u.Remark }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);

            #endregion
        }
        #endregion

        #region 测试页面
        public ActionResult test()
        {
            return View();
        }
        #endregion

        #region 实体初始化
        
        private Employee initEntity(Employee employee)
        {
            var employeeList = EmployeeService.LoadEntities(u => 1 == 1);

            string empCode;
            var empCodeList = (from u in employeeList
                                  orderby u.Code descending
                                  select
                                      new { u.Code });
            if (empCodeList.Any())
            {
                empCode = empCodeList.Take(1).ToList()[0].Code;
                int intLs = int.Parse(empCode.Substring(3));
                intLs++;
                employee.Code = "JZ-"+intLs.ToString();
            }
            else
            {
                employee.Code = "JZ-1001";
            }
            employee.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            employee.SubTime = DateTime.Now;
            employee.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            employee.ModifiedTIme = DateTime.Now;
            employee.TakeEffect = true;
            employee.TakeEffectTime = DateTime.Now;
            employee.LoseEffectTime = new DateTime(9999, 12, 31);
            employee.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return employee;
        }

        private Family initFamily(Family family)
        {
            family.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            family.SubTime = DateTime.Now;
            family.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            family.ModifiedTIme = DateTime.Now;
            family.TakeEffect = true;
            family.TakeEffectTime = DateTime.Now;
            family.LoseEffectTime = new DateTime(9999, 12, 31);
            family.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return family;
        }

        private Experience initExperience(Experience experience)
        {
            experience.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            experience.SubTime = DateTime.Now;
            experience.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            experience.ModifiedTIme = DateTime.Now;
            experience.TakeEffect = true;
            experience.TakeEffectTime = DateTime.Now;
            experience.LoseEffectTime = new DateTime(9999, 12, 31);
            experience.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return experience;
        }

        private Train initTrain(Train train)
        {
            train.SubBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            train.SubTime = DateTime.Now;
            train.ModifiedBy = LoginUserInfo == null ? 0 : LoginUserInfo.ID;
            train.ModifiedTIme = DateTime.Now;
            train.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            return train;
        }

        #endregion
    }
}
;