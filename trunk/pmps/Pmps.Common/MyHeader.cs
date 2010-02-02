using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;

namespace Pmps.Common
{
    public class MyHeaderInternal 
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
}
