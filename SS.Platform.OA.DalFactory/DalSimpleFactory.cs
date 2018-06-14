using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SS.Platform.OA.Dal;
using SS.Platform.OA.IDal;

namespace SS.Platform.OA.DalFactory
{
    public class DalSimpleFactory
    {
        #region DbSession替换
        //public static IDal.IUserInfoDal GetUserInfoDal()
        //{
        //    //反射
        //    //return GetInstances("SS.Platform.OA.Dal", "SS.Platform.OA.Dal." + "UserInfoEFDal") as IUserInfoDal;
        //    return new UserInfoEFDal();
        //}
        #endregion
        

        //反射创建实例
        public static object GetInstances(string assemblyName,string typeName)
        {
            return Assembly.Load(assemblyName).CreateInstance(typeName);
        }
    }
}
