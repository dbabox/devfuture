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
                        db.ExecuteNonQuery(schema.CmdClearSourceTreeNodeTable, trans);
                         Trace.TraceInformation("ɾ��������{0}ʵ������.", schema.Tna_table_name);
                    }
                    
                    foreach (DataRowTvaNode tn in list)
                    {
                        if (!force)
                        {
                            db.SetParameterValue(schema.CmdIsExist, schema.Tna_id_field_name, tn.TNA_ID);
                            if (Convert.ToInt32(db.ExecuteScalar(schema.CmdIsExist, trans)) == 1)
                            {
                                //ִ�и��� 
                                foreach (DbParameter par in schema.CmdUpdate.Parameters)
                                {
                                    if (String.Compare(par.ParameterName, "new" + schema.Tna_id_field_name, true) == 0)
                                    {
                                        db.SetParameterValue(schema.CmdUpdate , par.ParameterName, tn.DataRow[schema.Tna_id_field_name]);
                                    }
                                    else if (String.Compare(par.ParameterName, schema.Tna_id_field_name, true) == 0)
                                    {
                                        db.SetParameterValue(schema.CmdUpdate, par.ParameterName, tn.Original_tna_id);
                                    }
                                    else
                                    {
                                        db.SetParameterValue(schema.CmdUpdate, par.ParameterName, tn.DataRow[par.ParameterName]);
                                    }
                                }
                                rc += db.ExecuteNonQuery(schema.CmdUpdate, trans);

                            }
                            else
                            {
                                Trace.TraceError("TNA_ID={0}�����ݿ��в�����", tn.TNA_ID);
                            }
                        }
                        else
                        {
                            //ִ�����
                            foreach (DbParameter par in schema.CmdAdd.Parameters)
                            {
                                db.SetParameterValue(schema.CmdAdd, par.ParameterName, tn.DataRow[par.ParameterName]);
                            }
                            rc += db.ExecuteNonQuery(schema.CmdAdd, trans);
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
