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
        public MyHeader myHeader;

      
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
    }
}
