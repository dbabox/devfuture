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
        private static readonly DateTime ExpireDay = new DateTime(2010, 5, 1);
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        protected void Application_Start(Object sender, EventArgs e)
        {
            log.Debug("启动服务...");
            //从存储中加载CA文件
            //Application["DF_CA_KEY"] = System.Configuration.ConfigurationManager.AppSettings["DF_CA_KEY"];
        }
      
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
           
            HttpApplication app = sender as HttpApplication;
           
            if (DateTime.Today >= ExpireDay)
            {
                
                log.ErrorFormat("服务器已过期，请购买正式版服务软件.");
                app.CompleteRequest();  
            }
            if (app != null)
            {
                log.DebugFormat("访问服务:{0}", app.Request.Url);
            }
#if ENABLE_CLK
           
            //校验服务器
            if (!DevFuture.Common.Security.DFLicence.LocalCA(Application["DF_CA_KEY"] as string))
            {
                log.WarnFormat("服务器授权码无效:{0}", Application["DF_CA_KEY"]);                
                app.CompleteRequest();              
                
            }
            else
            {
                log.DebugFormat("授权码有效:{0}", Application["DF_CA_KEY"]);

                //执行其他动作

            }
#endif
            

        }
    }
}