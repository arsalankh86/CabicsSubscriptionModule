using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class WindowsServiceLogging
    {
        public static void WriteEventLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                string Date = System.DateTime.Now.ToString("dd-MM-yyyy");
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFolder\\DemoService" + Date + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ":  " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {


            }

        }

    }
}
