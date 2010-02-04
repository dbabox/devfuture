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

        public string Video_Proto
        {
            get { return video_proto; }
            set { 
                video_proto = value;                 
            }
        }
        private string base_url;    //服务器基址URL

        /// <summary>
        /// 服务器基础地址
        /// </summary>
        public string Base_Url
        {
            get { return base_url; }
            set { base_url = value; }
        }
        private string video_url;   //视频相对URL
        /// <summary>
        /// 视频文件相对URL
        /// </summary>
        public string Video_Url
        {
            get { return video_url; }
            set { video_url = value; }
        }

        private string media_url;

        /// <summary>
        /// 最终的媒体URL
        /// </summary>
        public string Media_Url
        {
            get { return media_url; }
            set { media_url = value; }
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
                    base_url = sr.ReadLine();
                    video_url = sr.ReadLine();
                    media_url = sr.ReadLine();
                  
                }
            }
        }

        public void SaveServerURLCfg()
        {
            string prefix =null;
            if (video_proto == "FILE")
            {
                prefix = "///";
            }
            else
            {
                prefix = "//";
            }
            if (base_url.EndsWith("/") && video_url.StartsWith("/"))
            {
                media_url = String.Format("{0}:{1}{2}{3}", video_proto,prefix,
                base_url.Substring(0, base_url.Length - 1), video_url);
            }
            else if ((!base_url.EndsWith("/")) && (!video_url.StartsWith("/")))
            {
                media_url = String.Format("{0}:{1}/{2}{3}", video_proto, prefix,
                base_url, video_url);
            }
            else
            {
                media_url = String.Format("{0}:{1}{2}{3}", video_proto, prefix,
               base_url, video_url);
            }

            string path = System.IO.Path.Combine(startup_path, CFG_FILE_NAME);
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fi.Open(System.IO.FileMode.Create)))
            {
                sw.WriteLine(video_proto);
                sw.WriteLine(base_url);
                sw.WriteLine(video_url);
                sw.WriteLine(media_url);              
                
            }
            
        }



    }
}
