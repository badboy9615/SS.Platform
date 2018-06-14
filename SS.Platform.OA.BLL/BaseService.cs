using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Platform.OA.BLL
{
    public abstract class BaseService<T>where T:class,new()
    {
        public IDal.IDbSession DbSession = DalFactory.DbSessionFactory.GetCurrentDbSession();
        public IDal.IBaseDal<T> CurrentDal;//依赖抽象接口

        public int SaveChanges()
        {
            return DbSession.SaveChanges();
        }

        //要求所有的业务方法在执行之前必须给CurrentDal赋值
        //构造函数是在实例创建时就已经执行了
        public BaseService()
        {
            //强迫子类给CurentDal赋值
            SetCurrentDal();
        }
        //纯抽象方法，子类必须重写此方法
        public abstract void SetCurrentDal();
       
        public virtual T Add(T entity)//虚方法允许子类重写
        {
            //要拿到T对应的Dal
            return CurrentDal.Add(entity);
        }

        public virtual bool Update(T entity)
        {
            return CurrentDal.Update(entity);
            //return this.SaveChanges() > 0;
        }

        public virtual int GetRecordCoutn(Func<T, bool> whereLambda)//虚方法允许子类重写
        {
            return CurrentDal.GetRecordCoutn(whereLambda);
        }

        public virtual bool Delete(T entity)
        {
            return CurrentDal.Delete(entity);
        }

        public virtual int Delete(params int[] ids)
        {
            return CurrentDal.Delete(ids);
        }

        public IQueryable<T> LoadEntities(Func<T, bool> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Func<T, bool> whereLambda, Func<T, S> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities(pageSize, pageIndex, out total, whereLambda, orderbyLambda, isAsc);
        }
    }
}
