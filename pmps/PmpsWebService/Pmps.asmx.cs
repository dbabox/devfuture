using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Serialization;

namespace PmpsWebService
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    
    public class PmpsService : System.Web.Services.WebService
    {

        [WebMethod]
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
        public Pmps.Common.MoUser GetUser()
        {
            Pmps.Common.MoUser m = new Pmps.Common.MoUser();
            m.Age = 33;
            m.UserId = new Random().Next().ToString();
            m.UserName = new Random().Next().ToString()+"_NAME_";
            return m;
        }
    }
}
