using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Common.Logging;

namespace PmpsWebService
{
    public class Global : System.Web.HttpApplication
    {
        
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        protected void Application_Start(Object sender, EventArgs e)
        {
            log.Debug("启动服务...");
            //从存储中加载CA文件
            Application["DF_CA_KEY"] = System.Configuration.ConfigurationManager.AppSettings["DF_CA_KEY"];
        }
      
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
           
            HttpApplication app = sender as HttpApplication;           
            //校验服务器
            if (false==DevFuture.Common.Security.DFLicence.LocalCA(Application["DF_CA_KEY"] as string))
            {
                log.WarnFormat("服务器授权码无效:{0}", Application["DF_CA_KEY"]);
                app.Response.Write("<html><body><b>Error 0:Licence Invalid. </b></body></html>");
                app.CompleteRequest();              
                
            }                     

        }
    }
}