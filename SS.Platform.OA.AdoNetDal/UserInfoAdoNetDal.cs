using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Platform.OA.IDal;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.AdoNetDal
{
    public class UserInfoAdoNetDal//:IUserInfoDal
    {
        public UserInfo Add(UserInfo userInfo)
        {
            return null;
        }


        public bool Update(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }

        public int Delete(params Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserInfo> LoadUserInfos(Func<UserInfo, bool> whereLambda)
        {
            throw new NotImplementedException();
        }


        public IQueryable<UserInfo> LoadPageUserInfos<S>(int pageSize, int pageIndex, out int total, Func<UserInfo, bool> whereLambda, Func<UserInfo, S> orderbyLambda, bool isAsc)
        {
            throw new NotImplementedException();
        }


        public int Delete(params int[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserInfo> LoadEntities(Func<UserInfo, bool> whereLambda)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserInfo> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Func<UserInfo, bool> whereLambda, Func<UserInfo, S> orderbyLambda, bool isAsc)
        {
            throw new NotImplementedException();
        }
    }
}
