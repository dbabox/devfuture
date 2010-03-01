using System;
using System.Collections.Generic;
using System.Text;

namespace LiveTV
{
    public class MMSServerCFG
    {
        private static string startup_path;

        public static string Startup_Path
        {
            get { return MMSServerCFG.startup_path; }
            set { MMSServerCFG.startup_path = value; }
        }



        private const string CFG_FILE_NAME = "livetv.cfg";


        private string video_proto; //视频协议
        /// <summary>
        /// 视频协议类型
        /// </summary>
        public string Video_Proto
        {
            get { return video_proto; }
            set { 
                video_proto = value;                 
            }
        }

        private string pmps_base_url;    //服务器基址URL

        /// <summary>
        /// 服务地址，一般的，若部署在IIS的根下，则直接为IP，否则包括虚拟目录；
        /// </summary>
        public string Pmps_Base_Url
        {
            get { return pmps_base_url; }
            set { pmps_base_url = value; }
        }
        private string broadcast_url;   //视频相对URL
        /// <summary>
        /// 广播地址 ，最终表达为 mms://xxx/xxx/xxx.asx
        /// </summary>
        public string Broadcast_Url
        {
            get { return broadcast_url; }
            set { broadcast_url = value; }
        }

      

        public void LoadServerURLFromCfg()
        {
            string path = System.IO.Path.Combine(startup_path, CFG_FILE_NAME);
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Exists)
            {
                //读取服务器地址配置
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fi.OpenRead()))
                {
                    video_proto = sr.ReadLine();
                    pmps_base_url = sr.ReadLine();
                    broadcast_url = sr.ReadLine();
                }
            }
        }

        public void SaveServerURLCfg()
        {         
           
            string path = System.IO.Path.Combine(startup_path, CFG_FILE_NAME);
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fi.Open(System.IO.FileMode.Create)))
            {
                sw.WriteLine(video_proto);
                sw.WriteLine(pmps_base_url);
                sw.WriteLine(broadcast_url);                        
                
            }
            
        }



    }
}
