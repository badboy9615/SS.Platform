using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS.Platform.Common
{
    public class WorkFlowHelper
    {
        static string strCon = ConfigurationManager.ConnectionStrings["shit"].ConnectionString;

        /// <summary>
        /// 创建工作流，并启动工作流
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static WorkflowApplication CreatApplicationAndRun(Activity activity, IDictionary<string, object> paramsData)
        {
            //如果你想要使用工作流进行序列化和持久化。
            WorkflowApplication application = new WorkflowApplication(activity, paramsData);

            //持久化两种方式：1，持久化到Sql中。2，持久化到Xml文件中。

            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(strCon);
            //让当前的我们的applicaton实例跟 数据库关联一块
            application.InstanceStore = store;

            //当工作流空闲的时候立即让我们工作流进行卸载，之前先序列化到 咱们的  数据库里面去。
            application.PersistableIdle = arg => { return PersistableIdleAction.Unload; };

            application.Idle = (a) => { Console.WriteLine("工作流停下了..."); };

            application.Run();//启动一个新的线程帮助我们执行工作流。
            return application;
        }

        /// <summary>
        /// 让工作流继续往下执行的代码
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="instanceId"></param>
        /// <param name="bookMark"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WorkflowApplication ContinueWorkflow(Activity activity, Guid instanceId, Bookmark bookMark, Object value)
        {
            WorkflowApplication application = new WorkflowApplication(activity);

            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(strCon);
            //让当前的我们的applicaton实例跟 数据库关联一块
            application.InstanceStore = store;

            //当工作流空闲的时候立即让我们工作流进行卸载，之前先序列化到 咱们的  数据库里面去。
            application.PersistableIdle = arg => { return PersistableIdleAction.Unload; };


            application.Load(instanceId);
            application.ResumeBookmark(bookMark, value);
            return application;
        }
    }
}
