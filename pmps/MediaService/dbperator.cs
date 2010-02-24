using System;
using System.Collections.Generic;

using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;

namespace MediaService
{
    class dbperator:IDisposable
    {
        static private SQLiteConnection conn;
        private string datasource;

        public dbperator(string inputsource)
        {
            datasource = inputsource;
            conn = new SQLiteConnection();
            connectDB(); //这里启动了DB
        }

          

        //连接数据库
        private void connectDB()
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            conn.ConnectionString = connstr.ToString();
            conn.Open();
        }

        //创建表 - 表有4个字段
        public void createTable()
        {
            SQLiteCommand cmd = new SQLiteCommand();

            //创建之前先判断是否需要删除
            string sql = "drop table if exists MediaServIndex;";

            cmd.CommandText = sql;

            cmd.Connection = conn;

            cmd.ExecuteNonQuery();

            //创建表
            sql = "create table MediaServIndex(ServerName varchar(20) primary key,URL varchar(50),Description varchar(40),Duration time)";

            cmd.CommandText = sql;

            cmd.Connection = conn;

            cmd.ExecuteNonQuery();
        }


        //向表中插入数据
        public void insertTable(string servname, string urlstr)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandText = "REPLACE  into MediaServIndex(ServerName, URL, Description,duration) values(?,?,?,?)";
                DbParameter p1 = cmd.CreateParameter();
                DbParameter p2 = cmd.CreateParameter();
                DbParameter p3 = cmd.CreateParameter();
                DbParameter p4 = cmd.CreateParameter();

                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);

                p1.Value = servname;
                p2.Value = urlstr;
                p3.Value = " "; // 暂时保存一个空字符
                p4.Value = " "; // 暂时保存一个空字符
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        const string sql_dropTable = "drop table MediaServIndex";

        //删除数据表
        public void dropTable()
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.CommandText = sql_dropTable;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (conn != null) conn.Dispose();
        }

        #endregion
    }
}
