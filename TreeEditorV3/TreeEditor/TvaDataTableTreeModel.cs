/*
 * ���������
1=����2=��ͨ�ڵ�  3=�м��νڵ�
 9=���е�Ҷ�ӽڵ㣬������һ�ּ�����
11=ҳ�����ڵ㣨���⣩ 12=ҳ����ͨ����
��PDF p105ͼ5-23
 * 
 * ���нڵ㣬Ӧ��������Ҫ��
 * 1������ID���߼�ID���룬 IDӦ���νṹ�޹أ����еĽڵ��ƶ���Ҳ��Ӧ����ID��PID
 * 2�����ṹ������ID����
 * 3���ڵ�����ʵ�ʸ��ĵ��ǽڵ���ͬ�������е�λ�ã����ͬ��Ӱ���߼�ID
 * 
 * ����ƶ���
 * 1�������ƶ��ڵ��ԭ���������Ƴ�
 * 2�����±��ƶ��ڵ��PID
 * 3�������ƶ��ڵ���뵽Ŀ�Ĳ���
 * 4������Դ���߼�ID
 * 5������Ŀ�Ĳ��߼�ID
 * 
 * ͬ���ƶ���
 * 1�������ƶ��ڵ�ת�Ƶ�һ����ʱ�ڵ㣬����Դ��ڵ���null
 * 2����Ŀ�Ľڵ�ת�Ƶ�Դ��ڵ�
 * 3����Ŀ�Ľڵ��ֵָ���ƶ��ڵ�
 * ע�⣺����ڵ�֧�ָ��ƹ��ܣ�����ͨ�����Ƶķ�ʽ���ı�ֵ������ڵ��ǽṹ��
 * ����ڵ����࣬��������ü��ɣ�
 * 4�����±����߼�ID
 * 
 * 
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
 
 
using Aga.Controls.Tree;
 
 
 
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Diagnostics;

namespace DF.WinUI.CustomComponent
{
    /// <summary>
    /// ����TVA���ڵ㣬Tag��DataRow����Model(PONO)��
    /// </summary>
    public interface ITvaModelNode
    {
        object Key
        {
            get;
            set;
        }

        object ParentKey
        {
            get;
            set;
        }

        string LogicKey
        {
            get;
            set;
        }

        /// <summary>
        /// �ڵ��ı�
        /// </summary>
        string Text
        {
            get;
            set;
        }
        /// <summary>
        /// �ڵ��ͼ�꣬���ʵ���ǣ��ڹ���TreeModelʱ���ӻ�����Դ����������
        /// </summary>
        System.Drawing.Image Icon
        {
            get;
            set;
        }
        /// <summary>
        /// �ڵ�Я����ʵ��Model����
        /// </summary>
        object Tag
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ�ѡ��
        /// </summary>
        bool IsChecked
        {
            get;
            set;
        }


    }

    public interface ISchema
    {
        string ProviderName
        {
            get;
        }
        string ParameterPrefix
        {
            get;
        }
        string ConnectionString
        {
            get;
        }

        /// <summary>
        /// ����
        /// </summary>
        string Schema_Name
        {
            get;
        }
        /// <summary>
        /// ���˽ڵ���Ӿ䣬����Ϊ��.
        /// ����У���Ҫ���� WHERE ν�ʡ�
        /// </summary>
        string SQL_Record_Filter_Clause
        {
            get;
        }

        /// <summary>
        /// DataTableָ�����ı��ʽ
        /// </summary>
        string DataTable_Root_Filter_Expression
        {
            get;
        }

        /// <summary>
        /// ����
        /// </summary>
        string ColumnName_Key
        {
            get;            
        }
        /// <summary>
        /// ������������Ϊnull
        /// </summary>
        string ColumnName_ParentKey
        {
            get;            
        }
        /// <summary>
        /// �߼�ID�����в����Ϣ
        /// </summary>
        string ColumnName_LogicKey
        {
            get;
        }
        /// <summary>
        /// ��ʾ���ı�
        /// </summary>
        string ColumnName_Text
        {
            get;
        }
    }


    public class DbSchema : ISchema
    {
        private string providerName;
        private string parameterPrefix = "@";
        private string connectionString;
        private string schema_Name;
        private string filter_Clause;
        private string columnName_Key;
        private string columnName_ParentKey;
        private string columnName_LogicKey;
        private string columnName_Text;
        private string dataTable_Root_Filter_Expression;


        




        #region ISchema ��Ա

        public string ProviderName
        {
            get { return providerName; }
            set
            {
                providerName = value;
            }
        }

        public string ParameterPrefix
        {
            get { return parameterPrefix; }
            set { parameterPrefix = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public string Schema_Name
        {
            get {return schema_Name; }
            set { schema_Name = value; }
        }

        public string SQL_Record_Filter_Clause
        {
            get { return filter_Clause; }
            set { filter_Clause = value; }
        }

        public string ColumnName_Key
        {
            get { return columnName_Key ; }
            set { columnName_Key = value; }
        }

        public string ColumnName_ParentKey
        {
            get { return columnName_ParentKey; }
            set { columnName_ParentKey = value; }
        }

        public string ColumnName_LogicKey
        {
            get { return columnName_LogicKey; }
            set { columnName_LogicKey = value; }
        }

        public string ColumnName_Text
        {
            get {return columnName_Text; }
            set { columnName_Text = value; }
        }
        public string DataTable_Root_Filter_Expression
        {
            get {
                if (!String.IsNullOrEmpty(dataTable_Root_Filter_Expression))
                {
                    return dataTable_Root_Filter_Expression;
                }
                return String.Format("{0} is NULL or {0}=''", columnName_ParentKey);
            }
            set{dataTable_Root_Filter_Expression=value;}
        }
        #endregion
    }


    #region ���ͽӿ���
    abstract class TvaModelNode<T> : ITvaModelNode where T : class
    {
        protected readonly ISchema schema;
        protected T tag;
        public TvaModelNode(T tag_, ISchema schema_)
        {
            tag = tag_;
            schema = schema_;
        }

        #region ITvaModelNode ��Ա

        public abstract object Key
        {
            get;
            set;

        }

        public abstract object ParentKey
        {
            get;
            set;
        }

        public abstract string LogicKey
        {
            get;
            set;
        }


        public abstract string Text
        {
            get;
            set;

        }

        protected System.Drawing.Image icon_;
        public virtual System.Drawing.Image Icon
        {
            get
            {
                return icon_;
            }
            set
            {
                icon_ = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                T mo = value as T;
                if (mo == null)
                {
                    throw new ArgumentException("������Ч", "value");
                }
                tag = mo;
            }
        }

        protected bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
            }
        }

        #endregion
        public override string ToString()
        {
            return String.Format("Text={0} IsChecked={1} Tag={2},Key={3},ParentKey={4},LogicKey={5}", 
                Text, IsChecked,Tag, Key,ParentKey,LogicKey);
        }
    }
    #endregion


    class TvaDataRowNode : TvaModelNode<DataRow>
    {
        public TvaDataRowNode(DataRow tag, ISchema schema_)
            : base(tag, schema_)
        {
        }

        public override object Key
        {
            get
            {
                return tag[schema.ColumnName_Key];
            }
            set
            {
                tag[schema.ColumnName_Key] = value;
            }
        }

        public override object ParentKey
        {
            get
            {
                return tag[schema.ColumnName_ParentKey];
            }
            set
            {
                tag[schema.ColumnName_ParentKey] = value;
            }
        }

        public override string LogicKey
        {
            get
            {
                 return tag[schema.ColumnName_LogicKey] as string;
            }
            set
            {
                tag[schema.ColumnName_LogicKey] = value;
            }
        }


        public override string Text
        {
            get
            {
                return tag[schema.ColumnName_Text] as string;
            }
            set
            {
                tag[schema.ColumnName_Text] = value;
            }
        }
    }

    /// <summary>
    /// ����Ա�ķ���
    /// </summary>
    class Da4Schema
    {
        private static DbType TypeMap(string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "varchar": return DbType.String;
                case "nvarchar": return DbType.String;
                case "int": return DbType.Int32;
                case "integer": return DbType.Int32;
                default: return DbType.String;
            }

        }

        private readonly Database db;
        private readonly ISchema schema;  
        private DbCommand cmdAdd;
       
       
        public Da4Schema(ISchema schema_)
        {
            schema = schema_;            
            DbProviderFactory f = DbProviderFactories.GetFactory(schema.ProviderName);
            db = new GenericDatabase(schema.ConnectionString, f);
            AutoGenerateCmd();
           
        }

        

        public void AutoGenerateCmd()
        {
            string sql_GetTreeNodeDataTable = String.Format("SELECT * FROM {0} {2} ORDER BY {1}",
               schema.Schema_Name, schema.ColumnName_LogicKey, schema.SQL_Record_Filter_Clause);
            
            Trace.TraceInformation("��ȡ���нڵ�SQL:{0}", sql_GetTreeNodeDataTable);

 
            using (DbCommand cmd = db.GetSqlStringCommand(sql_GetTreeNodeDataTable))
            {
                using (IDataReader rd = db.ExecuteReader(cmd))
                {
                    DataTable dt = rd.GetSchemaTable();
                    #region SQL����                 

                    StringBuilder sbAdd = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();

                    sbAdd.AppendFormat("INSERT INTO {0} ( ", schema.Schema_Name);
                    string colName = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        colName = dt.Rows[i]["ColumnName"].ToString();
                        Trace.TraceInformation("{0}:������{1},����:{2}", i, colName, dt.Rows[i]["DataType"]);
           

                        sbAdd.AppendFormat("{0},", colName);
                        sbValues.AppendFormat("{0}{1},", schema.ParameterPrefix, colName);

                    }
                    
                    sbAdd.Remove(sbAdd.Length - 1, 1);
                    sbValues.Remove(sbValues.Length - 1, 1);
 
                    sbAdd.AppendFormat(" ) Values( {0} ) ", sbValues.ToString());

                    string sqlCmd_Add = sbAdd.ToString();
                    Trace.TraceInformation("Add sql��{0}", sqlCmd_Add);
                    #endregion
                    cmdAdd = db.GetSqlStringCommand(sqlCmd_Add);

                    string colDataTypeName = null;
                    if (schema.ProviderName == "System.Data.OracleClient")
                    {
                        colDataTypeName = "DataType";
                    }
                    else
                    {
                        colDataTypeName = "DataTypeName";
                    }
                    string typename = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        colName = dt.Rows[i]["ColumnName"].ToString();

                        typename = dt.Rows[i][colDataTypeName].ToString();

                        Trace.TraceInformation("{0}:������{1},����:{2}", i, colName, typename);                        
                        db.AddInParameter(cmdAdd, colName, TypeMap(typename));

                    }
                    rd.Close();
                }

            }            
 
        }

        
        public DataSet GetTreeDs()
        {
            DbCommand cmd = db.GetSqlStringCommand(String.Format("SELECT * FROM {0} {2} ORDER BY {1}",
               schema.Schema_Name, schema.ColumnName_LogicKey, schema.SQL_Record_Filter_Clause));
            return db.ExecuteDataSet(cmd);
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
            string sql_Delete = String.Format("DELETE FROM {0} {1} ", schema.Schema_Name, delClause);
            Trace.TraceInformation("ɾ��ָ���������ӽڵ�:{0}", sql_Delete);
            try
            {
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DbCommand cmdDelete = db.GetSqlStringCommand( sql_Delete );
                            rc += db.ExecuteNonQuery(cmdDelete, trans);
                            foreach (DataRow row in dtTree.Rows)
                            {
                                foreach (DbParameter par in cmdAdd.Parameters)
                                {
                                    db.SetParameterValue(cmdAdd, par.ParameterName, row[par.ParameterName]);
                                }
                                rc += db.ExecuteNonQuery(cmdAdd, trans);
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
                throw ;

            }
            return rc;
        }

    }


    class TvaDataTableTreeModel : TreeModelBase
    {
        
        private const string Default_TVA_ROOT_KEY = "ROOT";
        readonly Da4Schema da;     
       
        //�ڵ㼯��Map���棬���ڿ���Ѱ���¼������������ṹ
        private Dictionary<string, ITvaModelNode[]> treeNodesMap;

        private Dictionary<string, ITvaModelNode> allNodesMap;
        /// <summary>
        /// ���нڵ㡣���ҽ���������ExpandAll֮�����������������нڵ㣻
        /// </summary>
        internal Dictionary<string, ITvaModelNode> AllNodesMap
        {
            get { return allNodesMap; }
           
        }

        /// <summary>
        /// ���ڵ�ӳ��
        /// </summary>
        internal Dictionary<string, ITvaModelNode[]> TreeNodesMap
        {
            get { return treeNodesMap; }          
        }
        //���������ʱ��ʹ�ô˱����������нڵ�ı�����
        private readonly DataTable dtTreeNodeMo;

        public DataTable DtTreeNodeMo
        {
            get { return dtTreeNodeMo; }            
        }
     
        private readonly ISchema schema;


        private bool autoMapLogicKey2Key = true;

        public bool AutoMapLogicKey2Key
        {
            get { return autoMapLogicKey2Key; }
            set { autoMapLogicKey2Key = value; }
        }


        /// <summary>
        /// ���������ͱ�Ԫ����Schema����ʵ����һ������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootFilter"></param>
        /// <param name="schema_"></param>
        public TvaDataTableTreeModel(DataTable dt, ISchema schema_)
        {
            schema = schema_;
            da = new Da4Schema(schema_);
            dtTreeNodeMo = dt;
            InitTreeModelViaDataTable(schema.DataTable_Root_Filter_Expression);
        }


        

        /// <summary>
        /// ͨ�����ڵ��DataTable.Select���ʽ������
        /// </summary>
        /// <param name="nodeFilterClause"></param>
        /// <param name="rootFilter"></param>
        public TvaDataTableTreeModel(ISchema schema_)             
        {
            schema = schema_;         
            da = new Da4Schema(schema_);
            dtTreeNodeMo = da.GetTreeDs().Tables[0];
            InitTreeModelViaDataTable(schema.DataTable_Root_Filter_Expression);
        }
        /// <summary>
        /// �����ݿ����
        /// </summary>
        public void LoadFromSchema()
        {
            dtTreeNodeMo.Clear();
            dtTreeNodeMo.AcceptChanges();
            foreach (DataRow row in da.GetTreeDs().Tables[0].Rows)
            {
                dtTreeNodeMo.ImportRow(row);
            }
            dtTreeNodeMo.AcceptChanges();
            InitTreeModelViaDataTable(schema.DataTable_Root_Filter_Expression);
            base.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));
        }
        /// <summary>
        /// ��ָ���ĸ������� , ����ͨ��ʵ�֣�����Ҫ������Դ����TreeEditor�ı�׼������
        /// �����߼����ID�ֶΣ����磺001001002... ��ʱ���߼����ID�ֶΣ����������ֶΡ�
        /// �����Ƽ���ô������Ϊ����һ����Ϊ��������ɸ��ġ�
        /// </summary>
        /// <param name="rootFilter"></param>
        private void InitTreeModelViaDataTable(string rootFilter)
        {
            DataRow[] rootRows = dtTreeNodeMo.Select(rootFilter);//
            if (treeNodesMap == null)
            {
                treeNodesMap = new Dictionary<string, ITvaModelNode[]>();
            }
            else
            {
                treeNodesMap.Clear();
            }
            if (allNodesMap == null)
            {
                allNodesMap = new Dictionary<string, ITvaModelNode>(dtTreeNodeMo.Rows.Count);
            }
            else
            {
                allNodesMap.Clear();
            }

            ITvaModelNode[] roots = new ITvaModelNode[rootRows.Length];//���ڵ���
            for (int i = 0; i < rootRows.Length; i++)
            {
                roots[i] = new TvaDataRowNode(rootRows[i],schema);
                allNodesMap.Add(roots[i].Key.ToString(), roots[i]);
                Trace.TraceInformation("��ʼ�����ڵ�{0}.", roots[i]);
            }
            lock (treeNodesMap)
            {
                treeNodesMap.Add(Default_TVA_ROOT_KEY, roots);
            }
                   
           
        }
        /// <summary>
        /// ͬ��ѡ�еĽڵ�
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        //public void SyncSelected(IList<DataRow> selected)
        //{
        //    foreach (ITvaModelNode n in allNodesMap.Values) n.IsChecked = false;
        //    if (selected != null)
        //    {
        //        foreach (DataRow m in selected)
        //        {
        //            allNodesMap[m.AssContId].IsChecked = true;
        //        }
        //    }
        //}

        #region ����TVA���ɹ��õķ���

        

        #region ����½ڵ㵽��

        /// <summary>
        /// ����½ڵ㵽ĳ��ָ�����׵�ĳ��ָ��λ�á�
        /// </summary>
        /// <param name="node">�������Key���ı���</param>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ITvaModelNode Add(ITvaModelNode node, TreePath parent, int index,bool forMove)
        {
            //��һ��Ҫ�ı�
            node.ParentKey = (parent == TreePath.Empty) ? null : ((ITvaModelNode)parent.LastNode).Key;
            //ȷ�����׼�
            string parentId = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((ITvaModelNode)parent.LastNode).Key.ToString();

            ITvaModelNode[] newArr = null;
            if (treeNodesMap.ContainsKey(parentId))
            {
                //ԭ���鳤��
                ITvaModelNode[] oldArr = treeNodesMap[parentId];
                int oldLenth = oldArr.Length;
                newArr = new ITvaModelNode[oldLenth + 1];
                Array.Copy(oldArr, 0, newArr, 0, index);
                newArr[index] = node;
                Trace.TraceInformation("Ϊ�ڵ㸳����������ParentKey����ʱΨһKey:{0}", node);
                if (newArr.Length > (index + 1)) //����ûcopy��
                {
                    Array.Copy(oldArr, index, newArr, index + 1, oldArr.Length - index);
                }
                treeNodesMap[parentId] = newArr;
            }
            else
            {
                newArr = new ITvaModelNode[] { node };
                treeNodesMap.Add(parentId, newArr);
            }
            if (!forMove)
            {
                DataRow m = (DataRow)node.Tag;   
                allNodesMap.Add(node.Key.ToString(), node);//��ʱKey��GUID���������ظ� 
                dtTreeNodeMo.Rows.Add(m);
                dtTreeNodeMo.AcceptChanges();
            }                   
            
            OnStructureChanged(new TreeModelEventArgs(parent, new int[] { index }, new object[] { node }));

            return node;


        }




        #endregion

        #region ɾ���ڵ�
        /// <summary>
        /// ɾ��ĳ���ڵ��µ�ĳ���ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        internal ITvaModelNode RemoveNode(TreePath parent, ITvaModelNode node,bool forMove)
        {
            //ȷ�����׼�
            string partentKey = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((ITvaModelNode)parent.LastNode).Key.ToString();
            ITvaModelNode[] oldArr = treeNodesMap[partentKey];
            if (oldArr.Length == 1) //ֻ��1�����ӣ���ô�϶���node
            {
                System.Diagnostics.Trace.Assert(node.Key == oldArr[0].Key);
                Trace.TraceInformation("��{0}��ֻ��1�����ӣ���ɾ����:{1}", partentKey, node);
                //ִ��ɾ��
                treeNodesMap.Remove(partentKey);
            }
            else
            {
                ITvaModelNode[] newArr = new ITvaModelNode[oldArr.Length - 1];
                int removedNodeIndex = -1;
                for (int i = 0; i < oldArr.Length; i++)
                {
                    if (oldArr[i] == node || oldArr[i].Key == node.Key)
                    {
                        removedNodeIndex = i;
                        break;
                    }

                }
                Array.Copy(oldArr, 0, newArr, 0, removedNodeIndex);
                if (removedNodeIndex < (newArr.Length - 1)) //��ĩ�ڵ�
                {
                    Array.Copy(oldArr, removedNodeIndex + 1,
                        newArr, removedNodeIndex, oldArr.Length - removedNodeIndex); //copyʣ�µ�
                }
                //���Ѿ�ִ��ɾ��������Ż�ӳ���
                treeNodesMap[partentKey] = newArr;

            }
            //�������            
            OnNodesRemoved(new TreeModelEventArgs(parent, new object[] { node }));
            if (!forMove)
            {
                allNodesMap.Remove((string)node.Key);
                dtTreeNodeMo.Rows.Remove(node.Tag as DataRow);
            }
            return node;
        }
        /// <summary>
        /// ����ɾ��ʱʹ�ã���һ���ڵ㳹��ɾ��
        /// </summary>
        /// <param name="node"></param>
        internal void RemoveNodeCssWithRow(ITvaModelNode node)
        {
            if (treeNodesMap.ContainsKey((string)node.Key))
            {
                treeNodesMap.Remove((string)node.Key);
            }
            allNodesMap.Remove((string)node.Key);
            DataRow[] rows = dtTreeNodeMo.Select(String.Format("{1}='{0}'", node.Key,schema.ColumnName_Key));
            if (rows.Length == 1) dtTreeNodeMo.Rows.Remove(rows[0]);
        }

        /// <summary>
        /// ɾ���ִ��һ���ڵ㡣ɾ��ʵ���ǴӺ��Ӽ������Ƴ���
        /// </summary>
        /// <param name="parent">���ڵ�·��.</param>
        /// <param name="node">��ɾ���Ľڵ㣬ֻ����Ҷ�ӡ�</param>
        public void Remove(TreePath parent, ITvaModelNode node)
        {
            Stack<ITvaModelNode> removed = new Stack<ITvaModelNode>();
            GetChildCss(node, removed);
            while (removed.Count > 1)
            {
                ITvaModelNode n = removed.Pop();
                RemoveNodeCssWithRow(n);
            }
            RemoveNode(parent, node,false); //ɾ������ڵ㣬����Ҳһ��ɾ����

        }





        internal void GetChildCss(ITvaModelNode node, Stack<ITvaModelNode> stack)
        {
            stack.Push(node);
            if (treeNodesMap.ContainsKey((string)node.Key))
            {
                foreach (ITvaModelNode n in treeNodesMap[(string)node.Key])
                {
                    GetChildCss(n, stack);
                }
            }

        }


        #endregion

        #region �ƶ��ڵ�
        /// <summary>
        /// �ƶ���֪��node�ڵ㵽��һ���ڵ�֮�¡�
        /// </summary>
        /// <param name="fromPath">�������ƶ�</param>
        /// <param name="node">���ƶ��Ľڵ�</param>
        /// <param name="toPath">�ƶ���Ŀ�ĵط�</param>
        /// <param name="index">λ��Ŀ�ĵط���ͬ�㺢�ӵڼ�����</param>
        /// <returns>�������ձ��ƶ�����λ�õĽڵ�</returns>
        public ITvaModelNode Move(TreePath fromPath, ITvaModelNode node, TreePath toPath, int index)
        {
            //��ɾ��            
            return Add(RemoveNode(fromPath, node,true), toPath, index,true);

        }
        #endregion

        #region ��ȫ����ID��������
        /// <summary>
        /// �ݹ���º��ӵ��߼�ID
        /// </summary>
        /// <param name="node"></param>
        internal void UpdateChildLogicId(ITvaModelNode node)
        {
            if (treeNodesMap.ContainsKey(node.Key.ToString()))//���к���
            {
                ITvaModelNode[] child = treeNodesMap[node.Key.ToString()];
                for (int i = 0; i < child.Length; i++)
                {
                    child[i].LogicKey = String.Format("{0}{1,3:D3}", node.LogicKey, i + 1);
                    //���¶�Ӧ����
                    dtTreeNodeMo.Select(String.Format("'{0}'={1}", child[i].Key, schema.ColumnName_Key))[0][schema.ColumnName_LogicKey] = child[i].LogicKey;
                    UpdateChildLogicId(child[i]);
                }
            }
        }

        /// <summary>
        /// ������ID
        /// </summary>
        public void FullUpdateID()
        {
            if (allNodesMap.Count > 0)
            {
                allNodesMap.Clear();
                ITvaModelNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];                
       
                for (int i = 0; i < root.Length; i++) //���¸��ڵ���߼�Key
                {
                    root[i].LogicKey = String.Format("{0,3:D3}", i + 1);
                    DataRow row = dtTreeNodeMo.Select(String.Format("{1}='{0}'", root[i].Key, schema.ColumnName_Key))[0];
                    row[schema.ColumnName_LogicKey] = root[i].LogicKey;

                    allNodesMap.Add(root[i].LogicKey, root[i]);
                    UpdateChildLogicId(root[i]);//�ݹ���º��ӽ����߼�Key

                    if (AutoMapLogicKey2Key)
                    {
                        root[i].Key = root[i].LogicKey;//���߼�Keyͬ������Key
                        row[schema.ColumnName_Key] = root[i].Key;
                    }
                }
                if (AutoMapLogicKey2Key)
                {
                    foreach (DataRow row in dtTreeNodeMo.Rows)//�������н��д���
                    {
                        row[schema.ColumnName_Key] = row[schema.ColumnName_LogicKey];//�߼�ID��ֵ����������ID��
                        string lkey = row[schema.ColumnName_LogicKey].ToString();
                        if (lkey.Length > 3)
                        {
                            row[schema.ColumnName_ParentKey] = lkey.Substring(0, lkey.Length - 3);
                        }
                        else
                        {
                            row[schema.ColumnName_ParentKey] = null;
                        }
                    }
                }
                dtTreeNodeMo.AcceptChanges();//���е��߼�ID������ϣ���dtҲȫ��ͬ����map��ֻ��root�ڵ�    
            
                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }
        }

      
        /// <summary>
        /// ��������ID���߼�ID��������������DB�С�
        /// ��������ɵ���������ӵ�ǰ����
        /// </summary>
        public void FullUpdateIDAndSave()
        {
            if (allNodesMap.Count > 0)
            {
                allNodesMap.Clear();
                ITvaModelNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];
                //������ݿ��оõ�Key
                StringBuilder sbdel = new StringBuilder();
                sbdel.Append(" WHERE ");
                for (int i = 0; i < root.Length; i++)
                {
                    sbdel.AppendFormat(" {1} LIKE '{0}%' ", root[i].Key,schema.ColumnName_Key);
                    if (i < root.Length - 1) sbdel.Append(" OR ");
                }
                //����ɾ���ɵļ�¼
                Trace.TraceInformation("ɾ�����������ϼ����ӽڵ�:{0}", sbdel.ToString());
                for (int i = 0; i < root.Length; i++)
                {
                    root[i].LogicKey = String.Format("{0,3:D3}", i + 1);                  
                    allNodesMap.Add(root[i].Key.ToString(), root[i]);
                    UpdateChildLogicId(root[i]);//���º��ӽ����߼�Key
                    if (AutoMapLogicKey2Key)
                    {
                        root[i].Key = root[i].LogicKey;//���߼�Keyͬ������Key                       
                    }
                    
                }

                if (AutoMapLogicKey2Key) //��������ӳ�䣬��Ӧͬ�����¸�Key
                {
                    foreach (DataRow row in dtTreeNodeMo.Rows)
                    {
                        row[schema.ColumnName_Key] = row[schema.ColumnName_LogicKey];//�߼�ID��ֵ����������ID��
                        string lkey = row[schema.ColumnName_LogicKey].ToString();
                        if (lkey.Length > 3)
                        {
                            row[schema.ColumnName_ParentKey] = lkey.Substring(0, lkey.Length - 3);
                        }
                        else
                        {
                            row[schema.ColumnName_ParentKey] = null;
                        }
                    }
                }
                dtTreeNodeMo.AcceptChanges();//���е��߼�ID������ϣ���dtҲȫ��ͬ����map��ֻ��root�ڵ�    
                //��dt�洢������
                try
                {
                    da.SaveTree(sbdel.ToString(), dtTreeNodeMo);
                }
                catch (DbException ex)
                {
                    Trace.Fail("��������DBʧ�ܣ�", ex.Message);
                }
                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }

        }
        #endregion
        #endregion

        /// <summary>
        /// ͬ��DT����
        /// </summary>
        public void SyncDt2Tree()
        {
            InitTreeModelViaDataTable(schema.DataTable_Root_Filter_Expression);
            base.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));
            FullUpdateID();
        }


        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            lock (treeNodesMap)
            {
                if (treePath.IsEmpty())
                {
                    return treeNodesMap[Default_TVA_ROOT_KEY];
                }
                else
                {
                    ITvaModelNode n = treePath.LastNode as ITvaModelNode;//��ȡ���ĺ���
                    if (treeNodesMap.ContainsKey((string)n.Key))
                    {
                        return treeNodesMap[(string)n.Key];
                    }                 
                    else
                    {
                        //�����е�ֱ������
                        DataRow[] rows = dtTreeNodeMo.Select(String.Format("{0}='{1}'",
                            schema.ColumnName_ParentKey, n.Key));
                        if (rows != null && rows.Length > 0)
                        {
                            ITvaModelNode[] nodeArray = new ITvaModelNode[rows.Length];
                            for (int r = 0; r < rows.Length; r++)
                            {                              
                                nodeArray[r] = new TvaDataRowNode(rows[r],schema);
                                allNodesMap.Add(nodeArray[r].Key.ToString(), nodeArray[r]);
                            }
                            lock (treeNodesMap)
                            {
                                treeNodesMap.Add(n.Key.ToString(), nodeArray);
                            }
                            return nodeArray;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }
            }
        }

        public override bool IsLeaf(TreePath treePath)
        {
            ITvaModelNode n = treePath.LastNode as ITvaModelNode;
            if (n == null) return true;
            return Convert.ToInt32(dtTreeNodeMo.Compute(String.Format("COUNT({0})", schema.ColumnName_Key), 
                String.Format("{0}='{1}'",schema.ColumnName_ParentKey, n.Key))) == 0;
        }


        

    }
}
