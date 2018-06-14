using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SS.Platform.OA.IDal;

namespace SS.Platform.OA.DalFactory
{
    public class DbSessionFactory
    {
        public static IDbSession GetCurrentDbSession()
        {
            //return   new DbSession();
            //保证线程内实例唯一
            IDbSession dbSession = (IDbSession)CallContext.GetData("DbSession");

            int threadId = Thread.CurrentThread.ManagedThreadId;

            if (dbSession == null)
            {
                dbSession = new DbSession();
                CallContext.SetData("DbSession", dbSession);
            }

            return dbSession;
            //return null;
        }
    }
}
