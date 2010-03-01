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


        private string video_proto; //��ƵЭ��
        /// <summary>
        /// ��ƵЭ������
        /// </summary>
        public string Video_Proto
        {
            get { return video_proto; }
            set { 
                video_proto = value;                 
            }
        }

        private string pmps_base_url;    //��������ַURL

        /// <summary>
        /// �����ַ��һ��ģ���������IIS�ĸ��£���ֱ��ΪIP�������������Ŀ¼��
        /// </summary>
        public string Pmps_Base_Url
        {
            get { return pmps_base_url; }
            set { pmps_base_url = value; }
        }
        private string broadcast_url;   //��Ƶ���URL
        /// <summary>
        /// �㲥��ַ �����ձ��Ϊ mms://xxx/xxx/xxx.asx
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
                //��ȡ��������ַ����
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
