using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;

namespace MediaService
{
    class Program
    {
        static readonly string CONFIG_INI = "config.ini";
        static string getFilePath()
        {
            FileInfo infoFile = new FileInfo(CONFIG_INI);
            StreamReader sr = infoFile.OpenText();
            string linestr = sr.ReadToEnd();

            //get path string
            int startPos = linestr.IndexOf("FILE");
            int endPos = 0;

            for (int i = startPos + 5; i < linestr.Length; i++)
            {
                if (linestr[i] == '\r')
                {
                    endPos = i;
                    break;
                }
            }

            //获取PATH子串
            string pathInfo = linestr.Substring(startPos + 5, endPos - startPos - 5);

            sr.Close();

            return pathInfo;
        }


        static string getDBPath()
        {
            FileInfo infoFile = new FileInfo(CONFIG_INI);
            StreamReader sr = infoFile.OpenText();
            string linestr = sr.ReadToEnd();

            //get path string
            int startPos = linestr.IndexOf("DB");
            int endPos = 0;

            for (int i = startPos + 3; i < linestr.Length; i++)
            {
                if (!char.IsLetter(linestr[i]))
                {
                    endPos = i;
                    break;
                }
            }

            //获取PATH子串
            string dbInfo = linestr.Substring(startPos + 3, linestr.Length - startPos - 3);

            sr.Close();

            return dbInfo;
        }

        //从文件中获取URL信息
        static string getURLInfo(FileInfo myfile)
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

        //主函数
        static void Main(string[] args)
        {

            //用于缓存文件名称，暂时没有特别用途
            List<string> Filelist = new List<string>();

            string strPath = getFilePath(); //文件夹的路径   
            DirectoryInfo dir = new DirectoryInfo(strPath);

            Console.WriteLine("The file watcher is started up");

            //建立数据库操作类
            dbperator dbopt = new dbperator(getDBPath());

            //创建表
            dbopt.createTable();

            foreach (FileInfo infoFile in dir.GetFiles("*.asx")) //遍历获得以asx为扩展名的文件   
            {
                string name;
                string urlinfo;

                name = infoFile.Name; //name为该文件夹下的文件名称 
                Filelist.Add(infoFile.Name);

                urlinfo = getURLInfo(infoFile);

                //去掉文件名中的扩展名部分，即'.asx'
                name = name.Substring(0, name.IndexOf('.'));

                //向表中加入数据
                dbopt.insertTable(name, urlinfo);
            }

            new FileWatchClass(strPath, dbopt);

            Console.WriteLine("按Q键退出..");
            while (Console.ReadKey(true).Key==ConsoleKey.Q)
            {
                //空操作，保证程序不退出
                break;
            }
        }
    }
}
