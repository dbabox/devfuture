using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Serialization;
using Pmps.Common;
using System.Collections.Generic;
using Common.Logging;
using Microsoft.WindowsMediaServices.Interop;
using System.Diagnostics;
 

namespace PmpsWebService
{
    /// <summary>
    /// 自定义的SOAP Header
    /// </summary>
    public class MyHeader : SoapHeader
    {
        private string uid;

        public string DF_Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string pwd;

        public string DF_Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        private string clientver;

        public string DF_Clientver
        {
            get { return clientver; }
            set { clientver = value; }
        }

    }

    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    
    public class PmpsService : System.Web.Services.WebService
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static int objCount=0;
        public MyHeader myHeader;
 

        public PmpsService()
        {
            ++objCount;
            log.DebugFormat("创建第{0}个对象,HashCode={1}", objCount,GetHashCode());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            --objCount;
            log.DebugFormat("释放对象{0},HashCode={1}", objCount, GetHashCode());
        }

       

      
        [WebMethod]
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.InOut)]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public int GetRandom()
        {
            return new Random().Next();
        }

        [WebMethod]     
        [XmlInclude(typeof(Pmps.Common.MoUser))] //在SOAP中指定应该返回的实际类型
        [SoapHeader("myHeader", Direction = SoapHeaderDirection.In)]
        public Pmps.Common.MoUser GetUser()
        {
            Pmps.Common.MoUser m = new Pmps.Common.MoUser();
            m.Age = 33;
            m.UserId = new Random().Next().ToString();
            m.UserName = new Random().Next().ToString()+"_NAME_";
            return m;
        }
        [WebMethod]    
        public int[] GetIntArray(int cnt)
        {
            int[] rc = new int[cnt];
            for (int i = 0; i < cnt; i++)
            {
                rc[i] = new Random().Next();
            }
            return rc;
        }

        [WebMethod]    
        [XmlInclude(typeof(Pmps.Common.MoUser))] 
        public Pmps.Common.MoUser[] GetUserArray(int cnt)
        {
            Pmps.Common.MoUser[] rc = new Pmps.Common.MoUser[cnt];
            for (int i = 0; i < cnt; i++)
            {
                rc[i] =new Pmps.Common.MoUser();
                rc[i].UserName = Guid.NewGuid().ToString();

            }
            return rc;
        }
        #region 使用文件监视器时
#if FILEWATCH
        const string SQL_GetMedialList = "SELECT SERVERNAME,URL,DESCRIPTION,DURATION FROM MEDIASERVINDEX";
        private static readonly System.Configuration.ConnectionStringSettings connSetting = System.Configuration.ConfigurationManager.ConnectionStrings["MediaInfo"];
        
        [WebMethod]
        [XmlInclude(typeof(Pmps.Common.MoMediaservindex))] 
        public Pmps.Common.MoMediaservindex[]  GetMedialList()
        {
            log.Debug("请求服务:GetMedialList");
            List<Pmps.Common.MoMediaservindex> list = new List<MoMediaservindex>();
                
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connSetting.ConnectionString))
            {
                conn.Open();
                log.Debug("连接打开");
                using (System.Data.SQLite.SQLiteTransaction trans = conn.BeginTransaction())
                {
                    log.Debug("启动事务");
                    using (System.Data.SQLite.SQLiteCommand cmd = conn.CreateCommand())//命令
                    {
                        log.Debug("创建命令");
                        cmd.CommandText = SQL_GetMedialList;
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = trans;
                        using (IDataReader rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                
                                Pmps.Common.MoMediaservindex m = new MoMediaservindex();
                                if (!rd.IsDBNull(0)) m.Servername = rd.GetString(0);
                                if (!rd.IsDBNull(1)) m.Url = rd.GetString(1);
                                if (!rd.IsDBNull(2)) m.Description = rd.GetString(2);
                                if(!rd.IsDBNull(3)) m.Duration = rd.GetDateTime(3);
                                list.Add(m);
                                log.DebugFormat("读取到：{0}", m);

                            }
                        }
                         
                    }
                    trans.Commit();
                }
                conn.Close();
            }
            return list.ToArray();
        }
#endif
        #endregion


        #region 下列方法返回MMS服务器的有效URL
        [WebMethod]  
        public string[] GetMedialUrl()
        {
            WMSServer server = null;
            try
            {
                #region 处理服务器发布点信息
                server = new WMSServerClass();
                foreach (string ip in server.AvailableIPAddresses)
                {
                    Trace.TraceInformation( "服务器有效IP:{0}", ip);
                }
                List<String> urlList = new List<string>();


                Trace.TraceInformation("服务器上共有{0}个发布点", server.PublishingPoints.Count);
                foreach (IWMSPublishingPoint p in server.PublishingPoints)
                {
                    switch (p.Type)
                    {
                        case WMS_PUBLISHING_POINT_TYPE.WMS_PUBLISHING_POINT_TYPE_ON_DEMAND:
                            {
                                //默认发布点 / ，以文件方式获取
                                Trace.TraceInformation("随需发布点({0}) :{1},状态:{2} ", p.Type, p.Name, p.Status);
                                Trace.TraceInformation("Path={0},WrapperPath={1}", p.Path, p.WrapperPath);
                                if (p.Status == WMS_PUBLISHING_POINT_STATUS.WMS_PUBLISHING_POINT_RUNNING)
                                {
                                    urlList.AddRange(GetUrlForPublishPoint(p.Path, GetServerPublicIP(server), p.Name));
                                }
                                break;
                            }
                        case WMS_PUBLISHING_POINT_TYPE.WMS_PUBLISHING_POINT_TYPE_CACHE_PROXY_BROADCAST:
                            {
                                //代理发布点
                                Trace.TraceInformation("缓存代理广播({0}) :{1},状态:{2}", p.Type, p.Name, p.Status);
                                break;
                            }
                        case WMS_PUBLISHING_POINT_TYPE.WMS_PUBLISHING_POINT_TYPE_CACHE_PROXY_ON_DEMAND:
                            {
                                Trace.TraceInformation("缓存代理随需发布点({0}) :{1},状态:{2}", p.Type, p.Name, p.Status);
                                break;
                            }
                        case WMS_PUBLISHING_POINT_TYPE.WMS_PUBLISHING_POINT_TYPE_BROADCAST:
                            {

                                Trace.TraceInformation("广播发布点({0}) :{1},状态:{2}", p.Type, p.Name, p.Status);
                                Trace.TraceInformation("Path={0},WrapperPath={1}", p.Path, p.WrapperPath);
                                if (p.Status == WMS_PUBLISHING_POINT_STATUS.WMS_PUBLISHING_POINT_RUNNING)
                                {
                                    urlList.AddRange(GetUrlForPublishPoint(p.Path, GetServerPublicIP(server), p.Name));
                                }
                                break;
                            }
                    }
                    Trace.TraceInformation("================================================");
                }
                #endregion

#if DEBUG
                Trace.TraceInformation("==============打印最终结果================");
                foreach (string url in urlList)
                {
                    Trace.TraceInformation(url);
                }
#endif
                //将urlList输出返回即可
                return urlList.ToArray();

            }
            finally
            {
                if (server != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(server);
                    server = null;
                }
            }
        }

        internal static string[] GetUrlForPublishPoint(string ppUri, string serverIp, string ppName)
        {
            List<string> urlList = new List<string>();
            string pp = ppName == "/" ? String.Empty : "/" + ppName;
            Uri u = new Uri(ppUri);
            if (System.IO.Directory.Exists(u.LocalPath))
            {
                string[] fiArr = System.IO.Directory.GetFiles(u.LocalPath);
                for (int i = 0; i < fiArr.Length; i++)
                {
                    urlList.Add(String.Format("mms://{0}{1}/{2}", serverIp, pp, System.IO.Path.GetFileName(fiArr[i])));
                }
            }
            else
            {
                urlList.Add(String.Format("mms://{0}{1}/{2}", serverIp, pp, System.IO.Path.GetFileName(u.LocalPath)));
            }
            return urlList.ToArray();
        }

        internal static string GetServerPublicIP(WMSServer server)
        {
            foreach (string ip in server.AvailableIPAddresses)
            {
                Trace.TraceInformation("服务器有效IP:{0}", ip);
                if (String.CompareOrdinal("127.0.0.1", ip) != 0) return ip;
            }
            return "127.0.0.1";

        }
        #endregion



    }
}
