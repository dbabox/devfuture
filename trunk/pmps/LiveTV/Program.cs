using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LiveTV
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            MMSServerCFG.Startup_Path = AppDomain.CurrentDomain.BaseDirectory;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //SZJ 开发机的授权码
            //string cca = "EAUMLMJBDXDTXDEOKXFLQIIRJBUPFDHCYDNHKWTDYPGBYCDXWHNYUSYMGEKAKZFMKROOJRKICUWVJNMRUEOJGKJFVEGDNTQVRQIGBASEPKPIYWVUCOXZWHALYGHZBXNKJXQIMLZJVYDDGUPTRRZMRJJQKACAXTXAEHMDPZYCFXDHRARHFOUZPFEKRJTHTFZFUXWWATVMKMKMYBNBMNLQSPWIVDKAZPIJ";

            
            Application.Run(new frmLiveTV());
        }
    }
}