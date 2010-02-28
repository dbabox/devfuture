using System;
using System.Collections.Generic;

using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;

namespace MediaService
{
    class FileWatchClass
    {
        const string filetype = "*.wsx";
        private FileSystemWatcher FileWatcher;
        private dbperator dbopt;
        private string watchpath;

          //从文件中获取URL信息
        private string getURLInfo(FileInfo myfile)
        {
            StreamReader sr = myfile.OpenText();

            string linestr = sr.ReadToEnd();
            int startPos = linestr.IndexOf("mms");
            int endPos = 0;

            for (int i = startPos + 1; i < linestr.Length; i++)
            {
                if (linestr[i] == '"')
                {
                    endPos = i;
                    break;
                }
            }

            //获取URL子串
            string urlInfo = linestr.Substring(startPos, endPos - startPos);

            sr.Close();

            return urlInfo;
        }

        public FileWatchClass(string pathConfig, dbperator dbsource)
        {
            watchpath = pathConfig;
            dbopt = dbsource;
            //删除表中数据
            dbopt.dropTable();
            //重新创建表
            dbopt.createTable();

            FileWatcher = new FileSystemWatcher();
            FileWatcher.Filter = filetype; //设定监听的文件类型
            FileWatcher.Path = watchpath; //设定监听的目录
            FileWatcher.Changed += new FileSystemEventHandler(filewatcherModified); //Changed 事件处理            
            FileWatcher.Created += new FileSystemEventHandler(filewatcherModified); //Created事件处理
            FileWatcher.Deleted += new FileSystemEventHandler(filewatcherModified); //Deleted事件处理
            //FileWatcher.Renamed += new RenamedEventHandler(filewatcherModified);//Renamed事件处理
            FileWatcher.IncludeSubdirectories = true;//不需监听子目录
            FileWatcher.EnableRaisingEvents = true;//开始进行监听
        }

        //监控处理函数，每当文件发生变动的时候，就重新生成数据表。就目前情况，数据量很小，速度能满足。
        void filewatcherModified(object sender, FileSystemEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(watchpath);           

            //重新向表中加入数据
            foreach (FileInfo infoFile in dir.GetFiles(filetype)) //遍历获得以asx为扩展名的文件   
            {
                string name;
                string urlinfo;

                name = infoFile.Name; //name为该文件夹下的文件名称 

                urlinfo = getURLInfo(infoFile);

                //去掉文件名中的扩展名部分，即'.asx'
                name = name.Substring(0, name.IndexOf('.'));

                //向表中加入数据
                dbopt.insertTable(name, urlinfo);
            }

        }
    }
}
