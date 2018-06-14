using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SS.Platform.OA.BLL;

namespace SS.Platform.WebService
{
    /// <summary>
    /// UserInfoServiceSoap 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class UserInfoServiceSoap : System.Web.Services.WebService,SS.Platform.OA.IBLL.IUserInfoService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}
        //[WebMethod]
        //public int Add(int a, int b)
        //{
        //    return a * b;
        //}
        UserInfoService userInfoService = new UserInfoService();
        [WebMethod]
        public SS.Platform.OA.Model.UserInfo Add(SS.Platform.OA.Model.UserInfo entity)
        {
            return userInfoService.Add(entity);
        }
        [WebMethod]
        public bool Update(SS.Platform.OA.Model.UserInfo entity)
        {
            return userInfoService.Update(entity);
        }
        [WebMethod]
        public bool Delete(SS.Platform.OA.Model.UserInfo entity)
        {
            return userInfoService.Delete(entity); 
        }
        //[WebMethod]
        public int Delete(params int[] ids)
        {
            return userInfoService.Delete(ids);
        }
       // [WebMethod]
        public IQueryable<SS.Platform.OA.Model.UserInfo> LoadEntities(Func<SS.Platform.OA.Model.UserInfo, bool> whereLambda)
        {
            return userInfoService.LoadEntities(whereLambda);
        }
        //[WebMethod]
        public IQueryable<SS.Platform.OA.Model.UserInfo> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Func<SS.Platform.OA.Model.UserInfo, bool> whereLambda, Func<SS.Platform.OA.Model.UserInfo, S> orderbyLambda, bool isAsc)
        {
            return userInfoService.LoadPageEntities(pageSize, pageIndex, out total, whereLambda, orderbyLambda, isAsc);
        }
        [WebMethod]
        public int SaveChanges()
        {
            return userInfoService.SaveChanges();
        }

        public int DeleteIds(params int[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OA.Model.UserInfo> LoadSearchData(OA.Model.BaseQueryParam param)
        {
            throw new NotImplementedException();
        }
    }
}
