using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// 行信息：ID，PID，Text列的列名。
    /// </summary>
    public class TvaSchema
    {
        private string tna_id_field_name="FUNC_ID";
        /// <summary>
        /// ID字段的名称
        /// </summary>
        public string Tna_id_field_name
        {
            get { return tna_id_field_name; }
            set { tna_id_field_name = value; }
        }
        private string tna_pid_field_name="PARENT_FUNC";
        /// <summary>
        /// 父ID字段的名称
        /// </summary>
        public string Tna_pid_field_name
        {
            get { return tna_pid_field_name; }
            set { tna_pid_field_name = value; }
        }
        private string tna_text_field_name="FUNC_NAME";
        /// <summary>
        /// 文本字段名称
        /// </summary>
        public string Tna_text_field_name
        {
            get { return tna_text_field_name; }
            set { tna_text_field_name = value; }
        }


        private string tna_logic_id_map_field="Order_Num";
        /// <summary>
        /// 逻辑ID映射字段，默认为null
        /// </summary>
        public string Tna_logic_id_map_field
        {
            get { return tna_logic_id_map_field; }
            set { tna_logic_id_map_field = value; }
        }


        private string tna_table_name="TFUNCTION";
        /// <summary>
        /// 树表名
        /// </summary>
        public string Tna_table_name
        {
            get { return tna_table_name; }
            set { tna_table_name = value; }
        }

        private string providerName = "System.Data.SqlClient";
        /// <summary>
        /// 数据库提供程序名称
        /// </summary>
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }
        private string connectionString="Data Source=.;Initial Catalog=eqt;UID=sa;PWD=admin";
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private string sql_GetGetNodesTotalCount;

        /// <summary>
        /// 获取总节点数,默认是 select count(*) from xxx.
        /// </summary>
        public string SQL_GetGetNodesTotalCount
        {
            get
            {
                if (String.IsNullOrEmpty(sql_GetGetNodesTotalCount))
                {
                    if (String.IsNullOrEmpty(tna_table_name)) throw new ArgumentNullException("tna_table_name");
                    sql_GetGetNodesTotalCount = String.Format("SELECT COUNT(*) FROM {0}", tna_table_name);
                }
                return sql_GetGetNodesTotalCount;
            }
            set
            {
                sql_GetGetNodesTotalCount = value;
            }
        }

        private string sql_GetTreeNodeDataTable;
        public string SQL_GetTreeNodeDataTable
        {
            get
            {
                if (String.IsNullOrEmpty(sql_GetTreeNodeDataTable))
                {
                    if (String.IsNullOrEmpty(tna_table_name)) throw new ArgumentNullException("tna_table_name");
                    sql_GetTreeNodeDataTable = String.Format("SELECT * FROM {0}", tna_table_name);
                }
                return sql_GetTreeNodeDataTable;
            }
            set
            {
                sql_GetTreeNodeDataTable = value;
            }
        }


        private string sql_ClearSourceTreeNodeTable;
        /// <summary>
        /// 清除源树表的SQL.
        /// </summary>
        public string SQL_ClearSourceTreeNodeTable
        {
            get {
                if (String.IsNullOrEmpty(sql_ClearSourceTreeNodeTable))
                {
                    if (String.IsNullOrEmpty(tna_table_name)) throw new ArgumentNullException("tna_table_name");
                    sql_ClearSourceTreeNodeTable = String.Format("Delete FROM {0} ", tna_table_name);
                }
                return sql_ClearSourceTreeNodeTable; 
            }
            set { sql_ClearSourceTreeNodeTable = value; }
        }

         

        private string sqlcmd_IsExist;
        public string SQLCmd_IsExist
        {
            get
            {
                if (String.IsNullOrEmpty(sqlcmd_IsExist))
                {
                    if (String.IsNullOrEmpty(tna_table_name)) throw new ArgumentNullException("tna_table_name");
                    if (String.IsNullOrEmpty(providerName)) throw new ArgumentNullException("providerName");
                    char parPrefix = ':';
                    if (providerName == "System.Data.SqlClient") parPrefix = '@';
                    sqlcmd_IsExist = String.Format("SELECT COUNT(*) FROM {0} WHERE {1}={2}{1} ", tna_table_name,
                        tna_id_field_name,parPrefix);
                }
                return sqlcmd_IsExist;
            }
            set
            {
                sqlcmd_IsExist = value;
            }
        }

        private string sqlCmd_Update = "UPDATE TFUNCTION SET FUNC_NAME=@FuncName, FUNC_CLASS=@FuncClass, FUNC_SIGN=@FuncSign, REM=@Rem, ORDER_NUM=@OrderNum, PARENT_FUNC=@ParentFunc WHERE FUNC_ID=@FuncId";
        /// <summary>
        /// 参数化的SQL命令，更新一行数据.
        /// 应从外部配置，未来考虑从数据库查询得到。
        /// Note:注意确保更新的顺序和查询的顺序是相同的。
        /// </summary>
        public string SQLCmd_Update
        {
            get { return sqlCmd_Update; }
            set { sqlCmd_Update = value; }
        }

        private string sqlCmd_Add = "INSERT INTO TFUNCTION ( TFUNCTION.FUNC_ID ,TFUNCTION.FUNC_NAME ,FUNC_CLASS ,FUNC_SIGN ,TFUNCTION.REM ,ORDER_NUM ,PARENT_FUNC   ) VALUES (@FuncId,@FuncName,@FuncClass,@FuncSign,@Rem,@OrderNum,@ParentFunc)";
        /// <summary>
        /// 添加一条记录所用的SQL。
        /// 应从外部配置，未来可实现直接从数据库元数据构造。
        /// Note:注意确保Add的顺序和查询的顺序是相同的。
        /// </summary>
        public string SQLCmd_Add
        {
            get { return sqlCmd_Add; }
            set { sqlCmd_Add = value; }
        }




        private DbType idFieldDbType=DbType.String;

        public DbType IdFieldDbType
        {
            get { return idFieldDbType; }
            set { idFieldDbType = value; }
        } 

        private string rowFilter_GetRootNodes;
        /// <summary>
        /// 从DataTable中得到根节点的RowFilter.
        /// 默认为 pid is NULL or pid=''
        /// </summary>
        public string RowFilter_GetRootNodes
        {
            get {
                if (string.IsNullOrEmpty(rowFilter_GetRootNodes))
                {
                    rowFilter_GetRootNodes = String.Format("{0} is NULL or {0}=''", tna_pid_field_name);
                }
                return rowFilter_GetRootNodes; 
            }
            set { rowFilter_GetRootNodes = value; }
        }

        


    }

}
