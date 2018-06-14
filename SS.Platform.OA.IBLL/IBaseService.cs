using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Platform.OA.IBLL
{
    public interface IBaseService<T>where T:class,new()
    {
        T Add(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        int GetRecordCoutn(Func<T, bool> whereLambda);

        int Delete(params int[] ids);

        //u=>true
        IQueryable<T> LoadEntities(Func<T, bool> whereLambda);//规约设计模式。 where a>10

        IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total,
                                                  Func<T, bool> whereLambda
                                                  , Func<T, S> orderbyLambda, bool isAsc);
        int SaveChanges();
    }
}
