using System;
using System.Data;
using System.Globalization;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Common.Logging;
using System.Collections.Generic;

namespace TreeEditor.Core
{
    public class DataTableTreeAdapter 
    {
        static readonly ILog log = LogManager.GetCurrentClassLogger();
         
        private Database db;
        private TvaSchema schema;

        public TvaSchema Schema
        {
            get { return schema; }
            set { schema = value; }
        }

     

        public DataTableTreeAdapter(TvaSchema schema_)
        {
            schema = schema_;

            DbProviderFactory f = DbProviderFactories.GetFactory(schema.ProviderName);
            db = new GenericDatabase(schema.ConnectionString, f); 
            
        }
        

        #region ITreeTableAdapter ��Ա 

        /// <summary>
        ///  ����ĺ�������Դ��ȡ����������Ĭ��ʵ����Select count(*) from xxx.
        /// </summary>
        /// <returns></returns>
        public int GetNodesTotalCount()
        {
            return Convert.ToInt32(db.ExecuteScalar(CommandType.Text, schema.SQL_GetGetNodesTotalCount));
        }

        /// <summary>
        /// ����ĺ�����������Դ��ȡ���нڵ�DataSet.
        /// </summary>
        /// <returns></returns>
        public System.Data.DataSet GetTreeNodeDataTable()
        {
            return db.ExecuteDataSet(CommandType.Text, schema.SQL_GetTreeNodeDataTable);
        }


        /// <summary>
        /// ����ĺ��������ڵ㱣�浽���ݿ�.
        /// </summary>
        /// <param name="treeNodeModelList"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public int SyncToDb(IEnumerable<DataRowTvaNode> list, bool force)
        {
            int rc = 0;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {                   
                    if (force)
                    {
                        DbCommand cmd_del_source = db.GetSqlStringCommand(schema.SQL_ClearSourceTreeNodeTable);
                        db.ExecuteNonQuery(cmd_del_source, trans);                         
                        if (log.IsWarnEnabled) log.WarnFormat("ɾ��������{0}ʵ������.", schema.Tna_table_name);
                    }
                    DbCommand cmd_is_Exist = db.GetSqlStringCommand(schema.SQLCmd_IsExist);
                    db.AddInParameter(cmd_is_Exist, schema.Tna_id_field_name, schema.IdFieldDbType);

                    DbCommand cmd_update = db.GetSqlStringCommand(schema.SQLCmd_Update);
                    db.DiscoverParameters(cmd_update);

                    DbCommand cmd_add = db.GetSqlStringCommand(schema.SQLCmd_Add);
                    db.DiscoverParameters(cmd_add);

                    foreach (DataRowTvaNode tn in list)
                    {
                        if (!force)
                        {
                            db.SetParameterValue(cmd_is_Exist, schema.Tna_id_field_name, tn.TNA_ID);
                            if (Convert.ToInt32(db.ExecuteScalar(cmd_is_Exist)) == 1)
                            {
                                //ִ�и��� 
                                foreach (DbParameter par in cmd_update.Parameters)
                                {
                                    db.SetParameterValue(cmd_update, par.ParameterName, tn.DataRow[par.ParameterName]);
                                }
                                rc += db.ExecuteNonQuery(cmd_update, trans);
                                
                            }
                            else
                            {
                                log.ErrorFormat("TNA_ID={0}�����ݿ��в�����", tn.TNA_ID);
                            }
                        }
                        else
                        {
                            //ִ�����
                            foreach (DbParameter par in cmd_add.Parameters)
                            {
                                db.SetParameterValue(cmd_add, par.ParameterName, tn.DataRow[par.ParameterName]);
                            }
                            rc += db.ExecuteNonQuery(cmd_add, trans);
                        }
                    }                  

                    trans.Commit();
                }
                conn.Close();
            }
            return rc;
        }

         
         
        

        #endregion


        
    }
}
