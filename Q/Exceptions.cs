using Serilog.Events;
using Q.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Q.Web
{
    public static class Exceptions
    {
        public static void HandleException(this MethodBase method, Exception ex)
        {
            Q.logger.Error($"Exception occured in method: {method.Name}");
            Q.logger.Error(ex.Message);
        }

        public static void HandleException(this MethodBase method, Exception ex, params string[] additionalInfo)
        {
            foreach(var info in additionalInfo)
            {
                Q.logger.Error(info);
            }
            HandleException(method, ex);
        }
    }
}
