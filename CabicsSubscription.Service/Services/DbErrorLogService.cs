using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class DbErrorLogService
    {
        DataContext context = new DataContext();
        public static void LogError(string module, string function, string argument, string error)
        {
            using (DataContext context = new DataContext())
            {
                DbErrorLog dbErrorLog = new DbErrorLog();
                dbErrorLog.ModuleName = module;
                dbErrorLog.ModuleFunction = function;
                dbErrorLog.FunctionArgument = argument;
                dbErrorLog.Error = error;
                dbErrorLog.IsActive = true;
                dbErrorLog.CreatedDate = DateTime.Now;
                context.DbErrorLogs.Add(dbErrorLog);
                context.SaveChanges();
            }
        }
    }
}
