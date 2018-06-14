using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SS.Platform.Common
{
    public class LogHelper
    {
        public static void WriteLog(string txt)
        {
            ILog log = log4net.LogManager.GetLogger("log4netlogger");

            log.Error(txt);

        }
    }
}
