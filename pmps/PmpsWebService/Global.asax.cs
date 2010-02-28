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
            log.Debug("��������...");
            //�Ӵ洢�м���CA�ļ�
            //Application["DF_CA_KEY"] = System.Configuration.ConfigurationManager.AppSettings["DF_CA_KEY"];
        }
      
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
           
            HttpApplication app = sender as HttpApplication;
           
            if (DateTime.Today >= ExpireDay)
            {
                
                log.ErrorFormat("�������ѹ��ڣ��빺����ʽ��������.");
                app.CompleteRequest();  
            }
            if (app != null)
            {
                log.DebugFormat("���ʷ���:{0}", app.Request.Url);
            }
#if ENABLE_CLK
           
            //У�������
            if (!DevFuture.Common.Security.DFLicence.LocalCA(Application["DF_CA_KEY"] as string))
            {
                log.WarnFormat("��������Ȩ����Ч:{0}", Application["DF_CA_KEY"]);                
                app.CompleteRequest();              
                
            }
            else
            {
                log.DebugFormat("��Ȩ����Ч:{0}", Application["DF_CA_KEY"]);

                //ִ����������

            }
#endif
            

        }
    }
}