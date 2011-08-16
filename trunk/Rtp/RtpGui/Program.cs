using System;
using System.Collections.Generic;
using System.Windows.Forms;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called ConsoleApp.exe.config in the application base
// directory (i.e. the directory containing ConsoleApp.exe)

namespace Rtp.Gui
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FrmOpenReader frm = new FrmOpenReader())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new FrmMain(frm.SelectedReader,frm.EnableDebug));
                }
            }
            
        }
    }
}