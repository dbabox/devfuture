using System;
using System.Data;
using System.Globalization;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using System.Collections.Generic;
using System.Diagnostics;

namespace TreeEditor.Core
{
    public class DataTableTreeAdapter 
    {
        
         
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
            schema.AutoGenerateCmd(db);
            
        }
        

        #region ITreeTableAdapter ��Ա 

        /// <summary>
        ///  ����ĺ�������Դ��ȡ����������Ĭ��ʵ����Select count(*) from xxx.
        /// </summary>
        /// <returns></returns>
        public int GetNodesTotalCount()
        {
            return Convert.ToInt32(db.ExecuteScalar( schema.CmdGetGetNodesTotalCount));
        }

        /// <summary>
        /// ����ĺ�����������Դ��ȡ���нڵ�DataSet.
        /// </summary>
        /// <returns></returns>
        public System.Data.DataSet GetTreeNodeDataTable()
        {
            return db.ExecuteDataSet(schema.CmdGetTreeNodeDataTable);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="delClause"></param>
        /// <param name="dtTree"></param>
        /// <returns></returns>
        public int SaveTree(string delClause, DataTable dtTree)
        {
            int rc = 0;
            try
            {
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {                        
                            DbCommand cmd_del = db.GetSqlStringCommand(String.Format("DELETE FROM {0} WHERE {1}", schema.Tna_table_name, delClause));
                            rc+=db.ExecuteNonQuery(cmd_del, trans);


                            //ִ�����

                            foreach (DataRow row in dtTree.Rows)
                            {
                                foreach (DbParameter par in schema.CmdAdd.Parameters)
                                {
                                    db.SetParameterValue(schema.CmdAdd, par.ParameterName, row[par.ParameterName]);
                                }
                                rc += db.ExecuteNonQuery(schema.CmdAdd, trans);
                            }

                          


                            trans.Commit();
                        }
                        catch (DbException tex)
                        {
                            if (trans != null)
                            {
                                trans.Rollback();
                                rc = 0;
                            }
                            Trace.Fail("����ʧ�ܡ�", tex.Message);
                            throw;
                        }
                    }
                    conn.Close();

                }
            }
            catch (DbException ex)
            {
                Trace.Fail("����ʧ�ܡ�", ex.Message);
                throw;

            }
            return rc;
        }



         
         
        

        #endregion


        
    }
}
