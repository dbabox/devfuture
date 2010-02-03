using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Serialization;
using Pmps.Common;

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
    }
}
