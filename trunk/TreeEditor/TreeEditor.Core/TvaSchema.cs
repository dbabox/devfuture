using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// ����Ϣ��ID��PID��Text�е�������
    /// </summary>
    public class TvaSchema
    {
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
        /// ��������SQL�������һ������.
        /// Ӧ���ⲿ���ã�δ�����Ǵ����ݿ��ѯ�õ���
        /// Note:ע��ȷ�����µ�˳��Ͳ�ѯ��˳������ͬ�ġ�
        /// </summary>
        public string SQLCmd_Update
        {
            get { return sqlCmd_Update; }
            set { sqlCmd_Update = value; }
        }

        private string sqlCmd_Add = "INSERT INTO TFUNCTION ( TFUNCTION.FUNC_ID ,TFUNCTION.FUNC_NAME ,FUNC_CLASS ,FUNC_SIGN ,TFUNCTION.REM ,ORDER_NUM ,PARENT_FUNC   ) VALUES (@FuncId,@FuncName,@FuncClass,@FuncSign,@Rem,@OrderNum,@ParentFunc)";
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




        private DbType idFieldDbType=DbType.String;

        public DbType IdFieldDbType
        {
            get { return idFieldDbType; }
            set { idFieldDbType = value; }
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

        


    }

}
