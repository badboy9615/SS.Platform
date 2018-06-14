using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using SS.Platform.OA.Dal;
using SS.Platform.OA.IDal;

namespace SS.Platform.OA.DalFactory
{
    /// <summary>
    /// DbSession:本质就是一个简单工厂，就是这么一个简单工厂，但从抽象意义上来说，它就是整个数据库访问层的统一入口。
    /// 因为拿到了DbSession就能够拿到整个数据库访问层所有的dal。
    /// </summary>
    public partial class DbSession : IDbSession
    {
        #region 这块由T4自动生成
        //public IUserInfoDal UserInfoDal
        //{
        //    get
        //    {
        //        return new UserInfoEFDal();
        //    }
        //}
        //public IRoleDal RoleDal
        //{
        //    get
        //    {
        //        return new RoleDal();
        //    }
        //}
        #endregion
        /// <summary>
        /// 跨世纪的。
        /// 能实现一个设计模式：  UnitWork
        /// 能够做到让开发人员可以随意在BLL层空数据库访问进行批量的提交。
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            //这里只需要调用当前线程内部的上下文SaveChange就完事了。
            DbContext dbContext = EFDbContextFactory.GetCurrentDbContext();


            return dbContext.SaveChanges();
        }


    }
}
