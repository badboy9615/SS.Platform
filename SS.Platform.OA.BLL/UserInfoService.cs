using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Platform.OA.AdoNetDal;
using SS.Platform.OA.Dal;
using SS.Platform.OA.DalFactory;
using SS.Platform.OA.IDal;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.BLL
{
    public partial class UserInfoService:BaseService<UserInfo>,IBLL.IUserInfoService
    {
        //ef实现
        //private UserInfoEFDal _userInfoEFDal = new UserInfoEFDal();
        //ADO.Net
        //private UserInfoAdoNetDal _userInfoEFDal = new UserInfoAdoNetDal();
        //依赖接口
        //IUserInfoDal userInfoDal = new UserInfoEFDal();
        //工厂模式
        //private IUserInfoDal userInfoDal = DalSimpleFactory.GetUserInfoDal();
        //DBSession
        //private IUserInfoDal userInfoDal = new DbSession().UserInfoDal;
        //DbSession dbSession = new DbSession();
        //IDbSession dbSession = new DbSession();
        //保证DBSession 线程内实例唯一
        //使用工厂
        #region 已在基类中统一实现

        //private IDbSession dbSession = DbSessionFactory.GetCurrentDbSession();
        //public UserInfo AddUserInfo(UserInfo userInfo)
        //{
        //    //用户注册场景
        //    //用户注册完成之后，给添加一个角色
        //    //dbSession.RoleDal.Add(new Role());
        //    //dbSession.RoleDal.Add(new Role());
        //    //dbSession.SaveChanges();

        //    //dbSession.RoleDal.Update(new Role());
        //    //dbSession.RoleDal.Add(new Role());
        //    //dbSession.RoleDal.Delete(new Role());

        //    //操作UserInfo
        //    dbSession.UserInfoDal.Add(userInfo);
        //    dbSession.SaveChanges();
        //    return userInfo;
        //}
        #endregion
        #region 已在T4生成代码文件中包含
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = DbSession.UserInfoDal;
        //}
        #endregion


        private string _Name = DateTime.Now.ToString();

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        #region 批量删除
        /// <summary>
        /// addeb by  laoma
        /// add a method to delete userGroups by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteIds(params int[] ids)
        {
            foreach (var id in ids)
            {
                var userInfo = DbSession.UserInfoDal.LoadEntities(u => u.ID == id).FirstOrDefault();
                userInfo.DelFlag = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Deleted;
            }
            return DbSession.SaveChanges();
        }


        #endregion

        #region 搜索查询
        public IQueryable<UserInfo> LoadSearchData(BaseQueryParam param)
        {
            //首先第一步先进行过滤

            short delNoarml = (short)SS.Platform.OA.Model.Enum.DelFlagEnum.Normal;
            //过滤删除的
            var temp = DbSession.UserInfoDal.LoadEntities(u => u.DelFlag == delNoarml);

            //过滤用户组名搜索的
            if (!string.IsNullOrEmpty(param.SName))
            {
                temp = temp.Where(u => u.Name.Contains(param.SName));
            }

            //设置总条数
            param.Total = temp.Count();

            //分页的处理
            return temp.OrderBy(u => u.ID).Skip(param.PageSize * (param.PageIndex - 1)).Take(param.PageSize).AsQueryable();
        }
        #endregion
    }
}
