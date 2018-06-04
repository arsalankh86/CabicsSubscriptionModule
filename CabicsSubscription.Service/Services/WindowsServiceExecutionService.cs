using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service.Services
{
    public class WindowsServiceExecutionService
    {
        DataContext context = new DataContext();
        public void InsertWindowsServiceExecutionService(WindowsServiceExecution windowsServiceExecution)
        {
           
            context.WindowsServiceExecutions.Add(windowsServiceExecution);
            context.SaveChanges();

        }

        public List<WindowsServiceExecution> GetAllPendingExecution()
        {
            return context.WindowsServiceExecutions.Where(x => x.WindowsServiceStatus == (int)Constant.WindowsServiceExecutionStatus.Pending && x.IsActive == true).ToList();
        }

    }
}
