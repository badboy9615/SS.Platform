using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memcached.ClientLibrary;

namespace SS.Platform.Common
{
    public class CacheHelper
    {
        public static readonly MemcachedClient mc;
        static CacheHelper()
        {
            string str = ConfigurationManager.AppSettings["MemcacheServers"];

            string[] serverlist = str.Split(',');

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }

        public static bool Insert(string key, object value)
        {
            //HttpContext.Current.Cache.Insert(key,value);
            mc.Set(key, value);
            return true;
        }

        public static bool Insert(string key, object value, DateTime expTime)
        {
            //HttpContext.Current.Cache.Insert(key,value,null,expTime,TimeSpan.Zero);
            mc.Set(key, value, expTime);
            return true;
        }

        public static object GetByKey(string key)
        {
            //return HttpContext.Current.Cache.Get(key);
            return mc.Get(key);
        }

        public static T Get<T>(string key)
        {
            //return (T)HttpContext.Current.Cache.Get(key);
            return (T)mc.Get(key);
        }

        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                mc.Delete(key);
            }
            return true;
        }
    }
}
