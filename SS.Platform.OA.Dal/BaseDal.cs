using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.Dal
{
    /// <summary>
    /// 把数据库访问层公共的方法抽出来进行实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDal<T> where T : class , new()
    {
        //上下文直接new无法保证上下文线程内实例唯一
        //改为使用上下文工厂获取
        //Model.DataModelContainer db = new DataModelContainer();
        //private DbContext db = EFDbContextFactory.GetCurrentDbContext();
        private DbContext db 
        {
            get
            {
                return EFDbContextFactory.GetCurrentDbContext(); 
            }
        }

        public virtual T Add(T entity)//虚方法允许子类重写
        {
            db.Set<T>().Add(entity);
            //db.SaveChanges();

            return entity;
        }

        public virtual int GetRecordCoutn(Func<T, bool> whereLambda)//虚方法允许子类重写
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda).Count();
        }

        public virtual bool Update(T entity)
        {
            //可以不用附加：

            //db.Set<T>().Attach(entity); 内部就是只是把实体的 状态改成unchange跟数据库查询出来的状态时一样的。
            db.Entry(entity).State = EntityState.Modified;

            //return db.SaveChanges() > 0;
            return true;
        }

        public virtual bool Delete(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;

            //return db.SaveChanges() > 0;
            return true;

        }

        public virtual int Delete(params int[] ids)
        {
            //T entity =new T();
            //entity.ID
            //首先可以通过  泛型的基类的约束来实现对id字段赋值。
            //也可也使用反射的方式。
            foreach (var item in ids)
            {
                var entity = db.Set<T>().Find(item);//如果实体已经在内存中，那么就直接从内存拿，如果内存中跟踪实体没有，那么才查询数据库。
                db.Set<T>().Remove(entity);
            }



            //return db.SaveChanges();
            return ids.Count();

        }

        public IQueryable<T> LoadEntities(Func<T, bool> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }

        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Func<T, bool> whereLambda, Func<T, S> orderbyLambda, bool isAsc)
        {
            total = db.Set<T>().Where(whereLambda).Count();

            if (isAsc)
            {
                return
                db.Set<T>()
                  .Where(whereLambda)
                  .OrderBy(orderbyLambda)
                  .Skip(pageSize * (pageIndex - 1))
                  .Take(pageSize)
                  .AsQueryable();
            }
            else
            {
                return
               db.Set<T>()
                 .Where(whereLambda)
                 .OrderByDescending(orderbyLambda)
                 .Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize)
                 .AsQueryable();
            }
        }
    }
}
