using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using SS.Platform.OA.Model;

namespace SS.Platform.OA.Dal
{
    public class EFDbContextFactory
    {
        /// <summary>
        /// 线程内实例唯一
        /// </summary>
        /// <returns></returns>
        public static DbContext GetCurrentDbContext()
        {
            //可以在这里切换模型
            //return new DataModel1Container();
            //return new DataModelContainer();

            //先去线程数据槽里去拿数据

            //if 有数据，直接返回

            //如果没有数据： 创建一个Ef上下文，然后放到数据槽里面去。返回数据。

            DbContext db = (DbContext)CallContext.GetData("DbContext");

            if (db == null)
            {
                db = new DataModelContainer();
                //db.Configuration.LazyLoadingEnabled = false;
                //db.Configuration.ProxyCreationEnabled = false;
                CallContext.SetData("DbContext", db);
            }
            return db;
        }
    }
}
