using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Pmps.Common
{
    [SoapType("SoapMoUserType", "http://www.cohowinery.com")]
    public class MoUser
    {
        string userId;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        int age;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }
}
