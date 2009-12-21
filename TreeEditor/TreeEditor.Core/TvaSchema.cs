using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Common.Logging;


namespace TreeEditor.Core
{
    /// <summary>
    /// ����Ϣ��ID��PID��Text�е�������
    /// </summary>
    public class TvaSchema
    {

        private static Dictionary<String, String> providerDic = new Dictionary<string, string>();

        public static Dictionary<String, String> ProviderDic
        {
            get { return TvaSchema.providerDic; }
            
        }

        static TvaSchema()
        {
            providerDic.Add("SQL Server", "System.Data.SqlClient");
            providerDic.Add("Oracle", "Oracle.DataAccess.Client");
        }

        static readonly ILog log = LogManager.GetCurrentClassLogger();

        private string tna_id_field_name="FUNC_ID";
        /// <summary>
        /// ID�ֶε�����
        /// </summary>
        public string Tna_id_field_name
        {
            get { return tna_id_field_name; }
            set { tna_id_field_name = value; }
        }
        private string tna_pid_field_name="PARENT_FUNC";
        /// <summary>
        /// ��ID�ֶε�����
        /// </summary>
        public string Tna_pid_field_name
        {
            get { return tna_pid_field_name; }
            set { tna_pid_field_name = value; }
        }
        private string tna_text_field_name="FUNC_NAME";
        /// <summary>
        /// �ı��ֶ�����
        /// </summary>
        public string Tna_text_field_name
        {
            get { return tna_text_field_name; }
            set { tna_text_field_name = value; }
        }


        private string tna_logic_id_map_field="Order_Num";
        /// <summary>
        /// �߼�IDӳ���ֶΣ�Ĭ��Ϊnull
        /// </summary>
        public string Tna_logic_id_map_field
        {
            get { return tna_logic_id_map_field; }
            set { tna_logic_id_map_field = value; }
        }


        private string tna_table_name="TFUNCTION";
        /// <summary>
        /// ������
        /// </summary>
        public string Tna_table_name
        {
            get { return tna_table_name; }
            set { tna_table_name = value; }
        }

        private string providerName = "System.Data.SqlClient";
        /// <summary>
        /// ���ݿ��ṩ��������
        /// </summary>
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }
        private string connectionString="Data Source=.;Initial Catalog=eqt;UID=sa;PWD=admin";
        /// <summary>
        /// �����ַ���
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private string sql_GetGetNodesTotalCount;
        /// <summary>
        /// ��ȡ�ܽڵ���,Ĭ���� select count(*) from xxx.
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
        /// ���Դ�����SQL.
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

        private  char parPrefix = ':';
        private char ParameterPrefix
        {
            get
            {                
                if (providerName == "System.Data.SqlClient") parPrefix = '@';
                return parPrefix;
            }
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
                   
                    sqlcmd_IsExist = String.Format("SELECT COUNT(*) FROM {0} WHERE {1}={2}{1} ", tna_table_name,
                        tna_id_field_name, ParameterPrefix);
                }
                return sqlcmd_IsExist;
            }
            set
            {
                sqlcmd_IsExist = value;
            }
        }


        #region �ɴ����ݿ�Ԫ���ݻ��
        private string sqlCmd_Update ;
        /// <summary>
        /// ��������SQL�������һ������.
        /// Ӧ���ⲿ���ã�δ�����Ǵ����ݿ��ѯ�õ���
        /// Note:ע��ȷ�����µ�˳��Ͳ�ѯ��˳������ͬ�ġ�
        /// </summary>
        public string SQLCmd_Update
        {
            get { return sqlCmd_Update; }
            set { sqlCmd_Update = value; }
        }

        private string sqlCmd_Add ;
        /// <summary>
        /// ���һ����¼���õ�SQL��
        /// Ӧ���ⲿ���ã�δ����ʵ��ֱ�Ӵ����ݿ�Ԫ���ݹ��졣
        /// Note:ע��ȷ��Add��˳��Ͳ�ѯ��˳������ͬ�ġ�
        /// </summary>
        public string SQLCmd_Add
        {
            get { return sqlCmd_Add; }
            set { sqlCmd_Add = value; }
        }

        #endregion


        private DbType idFieldDbType=DbType.String;

        public DbType IdFieldDbType
        {
            get { return idFieldDbType; }            
        } 

        private string rowFilter_GetRootNodes;
        /// <summary>
        /// ��DataTable�еõ����ڵ��RowFilter.
        /// Ĭ��Ϊ pid is NULL or pid=''
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

        private DbType TypeMap(string type)
        {
            switch (type)
            {
                case "varchar": return DbType.String;
                case "nvarchar": return DbType.String;
                case "int": return DbType.Int32;
                case "integer": return DbType.Int32;
                default: return DbType.String;
            }

        }

        #region ���е�SQL���� DbCommand ����
        private DbCommand cmdIsExist;

        public DbCommand CmdIsExist
        {
            get { return cmdIsExist; }
            
        }

        private DbCommand cmdGetTreeNodeDataTable;

        public DbCommand CmdGetTreeNodeDataTable
        {
            get { return cmdGetTreeNodeDataTable; }           
        }

        private DbCommand cmdGetGetNodesTotalCount;

        public DbCommand CmdGetGetNodesTotalCount
        {
            get { return cmdGetGetNodesTotalCount; }            
        }

        private DbCommand cmdAdd;

        public DbCommand CmdAdd
        {
            get { return cmdAdd; }            
        }
        private DbCommand cmdUpdate;

        public DbCommand CmdUpdate
        {
            get { return cmdUpdate; }
            
        }

        private DbCommand cmdClearSourceTreeNodeTable;

        public DbCommand CmdClearSourceTreeNodeTable
        {
            get { return cmdClearSourceTreeNodeTable; }

        }
        #endregion

        public void AutoGenerateCmd(Database db)
        {
            if (cmdAdd != null) cmdAdd.Dispose();
            if(cmdUpdate!=null) cmdUpdate.Dispose();

            using (DbCommand cmd = db.GetSqlStringCommand(this.SQL_GetTreeNodeDataTable))
            {
                using (IDataReader rd = db.ExecuteReader(cmd))
                { 
                    DataTable dt = rd.GetSchemaTable();
                    #region SQL����
                    StringBuilder sbUpdate = new StringBuilder();
                    sbUpdate.AppendFormat("Update {0} SET", Tna_table_name);
                    StringBuilder sbAdd = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();
                    
                    sbAdd.AppendFormat("INSERT INTO {0} ( ", Tna_table_name);
                    string colName = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        colName = dt.Rows[i]["ColumnName"].ToString();
                        log.DebugFormat("{0}:������{1},����:{2}", i, colName, dt.Rows[i]["DataType"]);
                        if (String.Compare(colName, Tna_id_field_name, true) != 0)
                        {
                            sbUpdate.AppendFormat(" {0}={1}{0},", colName, ParameterPrefix, colName);                          
                        }
                        else
                        {
                            sbUpdate.AppendFormat(" {0}={1}new{0},", colName, ParameterPrefix, colName); //����Ҳ���Ը���                           
                        }

                        sbAdd.AppendFormat("{0},", colName);
                        sbValues.AppendFormat("{0}{1},", ParameterPrefix, colName);
                       
                    }
                    sbUpdate.Remove(sbUpdate.Length - 1, 1);
                    sbAdd.Remove(sbAdd.Length - 1, 1);
                    sbValues.Remove(sbValues.Length - 1, 1);

                    sbUpdate.AppendFormat(" WHERE {0}={1}{0}", Tna_id_field_name, ParameterPrefix);
                    sbAdd.AppendFormat(" ) Values( {0} ) ",sbValues.ToString());

                    sqlCmd_Add = sbAdd.ToString();
                    sqlCmd_Update = sbUpdate.ToString();
                    log.DebugFormat("����sql��{0}", sqlCmd_Add);
                    log.DebugFormat("Add sql��{0}", sqlCmd_Update);
                    #endregion
                    cmdAdd = db.GetSqlStringCommand(sqlCmd_Add);
                    cmdUpdate = db.GetSqlStringCommand(sqlCmd_Update);

                    string typename=null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        colName = dt.Rows[i]["ColumnName"].ToString();
                        typename=dt.Rows[i]["DataTypeName"].ToString();
                        log.DebugFormat("{0}:������{1},����:{2}", i, colName,typename );
                        if (String.Compare(colName, Tna_id_field_name, true) != 0)
                        {                            
                            db.AddInParameter(cmdUpdate, colName, TypeMap(typename));
                        }
                        else
                        {                           
                            db.AddInParameter(cmdUpdate, "new" + colName, TypeMap(typename));
                            db.AddInParameter(cmdUpdate,colName, TypeMap(typename));

                            idFieldDbType = TypeMap(typename);
                        }                      
                        db.AddInParameter(cmdAdd, colName, TypeMap(typename));

                    }
                    rd.Close();
                }
               
            }

            cmdIsExist = db.GetSqlStringCommand(SQLCmd_IsExist);
            cmdGetTreeNodeDataTable = db.GetSqlStringCommand(SQL_GetTreeNodeDataTable);
            cmdGetGetNodesTotalCount = db.GetSqlStringCommand(SQL_GetGetNodesTotalCount);
            cmdClearSourceTreeNodeTable = db.GetSqlStringCommand(SQL_ClearSourceTreeNodeTable);
            db.AddInParameter(cmdIsExist, Tna_id_field_name, IdFieldDbType); 
        }
        
        


    }

}
