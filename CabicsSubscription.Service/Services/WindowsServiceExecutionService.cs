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

        public List<WindowsServiceExecution> GetAutoRenewelEntryBySubscriptionID(int subscriptionId)
        {
            // return context.WindowsServiceExecutions.Where(x => x.WindowsServiceStatus == (int)Constant.WindowsServiceExecutionStatus.Pending && x.IsActive == true).ToList();

            return null;
        }

        public void MarkWindowsServiceStatus(int windowsServiceExecutionId, int status)
        {
            using (DataContext context = new DataContext())
            {
                WindowsServiceExecution windowsServiceExecution = context.WindowsServiceExecutions.FirstOrDefault(x => x.Id == windowsServiceExecutionId);
                windowsServiceExecution.WindowsServiceStatus = status;
                windowsServiceExecution.UpdatedDate = DateTime.Now;
                context.SaveChanges();
            }
        }
    }
}
