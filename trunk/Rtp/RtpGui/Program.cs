using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FrmOpenReader frm = new FrmOpenReader())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new FrmMain(frm.SelectedReader));
                }
            }
            
        }
    }
}