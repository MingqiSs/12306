using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12036ByTicket
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ///register log4net
            log4net.Config.XmlConfigurator.Configure(
            new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\log4net.config")
            );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
