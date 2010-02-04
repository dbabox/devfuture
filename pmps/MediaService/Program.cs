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

            //��ȡPATH�Ӵ�
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

            //��ȡPATH�Ӵ�
            string dbInfo = linestr.Substring(startPos + 3, linestr.Length - startPos - 3);

            sr.Close();

            return dbInfo;
        }

        //���ļ��л�ȡURL��Ϣ
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

            //��ȡURL�Ӵ�
            string urlInfo = linestr.Substring(startPos, endPos - startPos);

            sr.Close();

            return urlInfo;
        }

        //������
        static void Main(string[] args)
        {

            //���ڻ����ļ����ƣ���ʱû���ر���;
            List<string> Filelist = new List<string>();

            string strPath = getFilePath(); //�ļ��е�·��   
            DirectoryInfo dir = new DirectoryInfo(strPath);

            Console.WriteLine("The file watcher is started up");

            //�������ݿ������
            dbperator dbopt = new dbperator(getDBPath());

            //������
            dbopt.createTable();

            foreach (FileInfo infoFile in dir.GetFiles("*.asx")) //���������asxΪ��չ�����ļ�   
            {
                string name;
                string urlinfo;

                name = infoFile.Name; //nameΪ���ļ����µ��ļ����� 
                Filelist.Add(infoFile.Name);

                urlinfo = getURLInfo(infoFile);

                //ȥ���ļ����е���չ�����֣���'.asx'
                name = name.Substring(0, name.IndexOf('.'));

                //����м�������
                dbopt.insertTable(name, urlinfo);
            }

            new FileWatchClass(strPath, dbopt);

            Console.WriteLine("��Q���˳�..");
            while (Console.ReadKey(true).Key==ConsoleKey.Q)
            {
                //�ղ�������֤�����˳�
                break;
            }
        }
    }
}
