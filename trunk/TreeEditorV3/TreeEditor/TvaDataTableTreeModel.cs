/*
 * 检查内容树
1=根，2=普通节点  3=中间层次节点
 9=树中的叶子节点，代表着一种检查分类
11=页面分组节点（标题） 12=页面普通内容
见PDF p105图5-23
 * 
 * 树中节点，应符合下列要求：
 * 1、物理ID和逻辑ID分离， ID应与层次结构无关，所有的节点移动，也不应更改ID，PID
 * 2、树结构用物理ID构建
 * 3、节点排序实际更改的是节点在同层数组中的位置，这会同步影响逻辑ID
 * 
 * 跨层移动：
 * 1、将被移动节点从原层数组中移除
 * 2、更新被移动节点的PID
 * 3、将被移动节点插入到目的层中
 * 4、更新源层逻辑ID
 * 5、更新目的层逻辑ID
 * 
 * 同层移动：
 * 1、将被移动节点转移到一个临时节点，并将源层节点置null
 * 2、将目的节点转移到源层节点
 * 3、将目的节点的值指向被移动节点
 * 注意：如果节点支持复制功能，可以通过复制的方式来改变值，例如节点是结构；
 * 如果节点是类，则更改引用即可；
 * 4、更新本层逻辑ID
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
    /// 代表TVA树节点，Tag是DataRow或者Model(PONO)。
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
        /// 节点文本
        /// </summary>
        string Text
        {
            get;
            set;
        }
        /// <summary>
        /// 节点的图标，最佳实践是：在构建TreeModel时，从环境资源中设置它。
        /// </summary>
        System.Drawing.Image Icon
        {
            get;
            set;
        }
        /// <summary>
        /// 节点携带的实际Model对象。
        /// </summary>
        object Tag
        {
            get;
            set;
        }
        /// <summary>
        /// 是否选中
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
        /// 表名
        /// </summary>
        string Schema_Name
        {
            get;
        }
        /// <summary>
        /// 过滤节点的子句，可以为空.
        /// 如果有，则要包含 WHERE 谓词。
        /// </summary>
        string SQL_Record_Filter_Clause
        {
            get;
        }

        /// <summary>
        /// DataTable指定根的表达式
        /// </summary>
        string DataTable_Root_Filter_Expression
        {
            get;
        }

        /// <summary>
        /// 主键
        /// </summary>
        string ColumnName_Key
        {
            get;            
        }
        /// <summary>
        /// 父级键，可以为null
        /// </summary>
        string ColumnName_ParentKey
        {
            get;            
        }
        /// <summary>
        /// 逻辑ID，具有层次信息
        /// </summary>
        string ColumnName_LogicKey
        {
            get;
        }
        /// <summary>
        /// 显示的文本
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


        




        #region ISchema 成员

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


    #region 泛型接口类
    abstract class TvaModelNode<T> : ITvaModelNode where T : class
    {
        protected readonly ISchema schema;
        protected T tag;
        public TvaModelNode(T tag_, ISchema schema_)
        {
            tag = tag_;
            schema = schema_;
        }

        #region ITvaModelNode 成员

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
                    throw new ArgumentException("类型无效", "value");
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
    /// 代表对表的访问
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
            
            Trace.TraceInformation("获取所有节点SQL:{0}", sql_GetTreeNodeDataTable);

 
            using (DbCommand cmd = db.GetSqlStringCommand(sql_GetTreeNodeDataTable))
            {
                using (IDataReader rd = db.ExecuteReader(cmd))
                {
                    DataTable dt = rd.GetSchemaTable();
                    #region SQL命令                 

                    StringBuilder sbAdd = new StringBuilder();
                    StringBuilder sbValues = new StringBuilder();

                    sbAdd.AppendFormat("INSERT INTO {0} ( ", schema.Schema_Name);
                    string colName = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        colName = dt.Rows[i]["ColumnName"].ToString();
                        Trace.TraceInformation("{0}:列名：{1},类型:{2}", i, colName, dt.Rows[i]["DataType"]);
           

                        sbAdd.AppendFormat("{0},", colName);
                        sbValues.AppendFormat("{0}{1},", schema.ParameterPrefix, colName);

                    }
                    
                    sbAdd.Remove(sbAdd.Length - 1, 1);
                    sbValues.Remove(sbValues.Length - 1, 1);
 
                    sbAdd.AppendFormat(" ) Values( {0} ) ", sbValues.ToString());

                    string sqlCmd_Add = sbAdd.ToString();
                    Trace.TraceInformation("Add sql：{0}", sqlCmd_Add);
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

                        Trace.TraceInformation("{0}:列名：{1},类型:{2}", i, colName, typename);                        
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
        /// 保存树
        /// </summary>
        /// <param name="delClause"></param>
        /// <param name="dtTree"></param>
        /// <returns></returns>
        public int SaveTree(string delClause, DataTable dtTree)
        {
            int rc = 0;
            string sql_Delete = String.Format("DELETE FROM {0} {1} ", schema.Schema_Name, delClause);
            Trace.TraceInformation("删除指定根及其子节点:{0}", sql_Delete);
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
                            Trace.Fail("事务失败。", tex.Message);
                            throw;
                        }
                    }
                    conn.Close();

                }
            }
            catch (DbException ex)
            {
                Trace.Fail("事务失败。", ex.Message);
                throw ;

            }
            return rc;
        }

    }


    class TvaDataTableTreeModel : TreeModelBase
    {
        
        private const string Default_TVA_ROOT_KEY = "ROOT";
        readonly Da4Schema da;     
       
        //节点集合Map缓存，用于快速寻找下级，并构建树结构
        private Dictionary<string, ITvaModelNode[]> treeNodesMap;

        private Dictionary<string, ITvaModelNode> allNodesMap;
        /// <summary>
        /// 所有节点。当且仅当树调用ExpandAll之后，它才真正包含所有节点；
        /// </summary>
        internal Dictionary<string, ITvaModelNode> AllNodesMap
        {
            get { return allNodesMap; }
           
        }

        /// <summary>
        /// 树节点映射
        /// </summary>
        internal Dictionary<string, ITvaModelNode[]> TreeNodesMap
        {
            get { return treeNodesMap; }          
        }
        //非随需加载时，使用此变量保存所有节点的表数据
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
        /// 给定树表，和表元数据Schema对象，实例化一个树。
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
        /// 通过根节点的DataTable.Select表达式加载树
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
        /// 从数据库加载
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
        /// 从指定的根加载树 , 考虑通用实现，这里要求树的源表警告TreeEditor的标准化处理，
        /// 包括逻辑层次ID字段，形如：001001002... 有时候，逻辑层次ID字段，就是主键字段。
        /// 但不推荐这么做，因为主键一旦成为外键，不可更改。
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

            ITvaModelNode[] roots = new ITvaModelNode[rootRows.Length];//根节点行
            for (int i = 0; i < rootRows.Length; i++)
            {
                roots[i] = new TvaDataRowNode(rootRows[i],schema);
                allNodesMap.Add(roots[i].Key.ToString(), roots[i]);
                Trace.TraceInformation("初始化根节点{0}.", roots[i]);
            }
            lock (treeNodesMap)
            {
                treeNodesMap.Add(Default_TVA_ROOT_KEY, roots);
            }
                   
           
        }
        /// <summary>
        /// 同步选中的节点
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

        #region 所有TVA树可共用的方法

        

        #region 添加新节点到树

        /// <summary>
        /// 添加新节点到某个指定父亲的某个指定位置。
        /// </summary>
        /// <param name="node">必须给定Key和文本。</param>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ITvaModelNode Add(ITvaModelNode node, TreePath parent, int index,bool forMove)
        {
            //父一定要改变
            node.ParentKey = (parent == TreePath.Empty) ? null : ((ITvaModelNode)parent.LastNode).Key;
            //确定父亲键
            string parentId = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((ITvaModelNode)parent.LastNode).Key.ToString();

            ITvaModelNode[] newArr = null;
            if (treeNodesMap.ContainsKey(parentId))
            {
                //原数组长度
                ITvaModelNode[] oldArr = treeNodesMap[parentId];
                int oldLenth = oldArr.Length;
                newArr = new ITvaModelNode[oldLenth + 1];
                Array.Copy(oldArr, 0, newArr, 0, index);
                newArr[index] = node;
                Trace.TraceInformation("为节点赋予了完整的ParentKey和临时唯一Key:{0}", node);
                if (newArr.Length > (index + 1)) //还有没copy的
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
                allNodesMap.Add(node.Key.ToString(), node);//此时Key是GUID，不可能重复 
                dtTreeNodeMo.Rows.Add(m);
                dtTreeNodeMo.AcceptChanges();
            }                   
            
            OnStructureChanged(new TreeModelEventArgs(parent, new int[] { index }, new object[] { node }));

            return node;


        }




        #endregion

        #region 删除节点
        /// <summary>
        /// 删除某个节点下的某个子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        internal ITvaModelNode RemoveNode(TreePath parent, ITvaModelNode node,bool forMove)
        {
            //确定父亲键
            string partentKey = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((ITvaModelNode)parent.LastNode).Key.ToString();
            ITvaModelNode[] oldArr = treeNodesMap[partentKey];
            if (oldArr.Length == 1) //只有1个孩子，那么肯定是node
            {
                System.Diagnostics.Trace.Assert(node.Key == oldArr[0].Key);
                Trace.TraceInformation("父{0}下只有1个孩子，且删除它:{1}", partentKey, node);
                //执行删除
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
                if (removedNodeIndex < (newArr.Length - 1)) //非末节点
                {
                    Array.Copy(oldArr, removedNodeIndex + 1,
                        newArr, removedNodeIndex, oldArr.Length - removedNodeIndex); //copy剩下的
                }
                //将已经执行删除的数组放回映射表
                treeNodesMap[partentKey] = newArr;

            }
            //真正清除            
            OnNodesRemoved(new TreeModelEventArgs(parent, new object[] { node }));
            if (!forMove)
            {
                allNodesMap.Remove((string)node.Key);
                dtTreeNodeMo.Rows.Remove(node.Tag as DataRow);
            }
            return node;
        }
        /// <summary>
        /// 级联删除时使用，将一个节点彻底删除
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
        /// 删除现存的一个节点。删除实际是从孩子集合中移除。
        /// </summary>
        /// <param name="parent">父节点路径.</param>
        /// <param name="node">被删除的节点，只能是叶子。</param>
        public void Remove(TreePath parent, ITvaModelNode node)
        {
            Stack<ITvaModelNode> removed = new Stack<ITvaModelNode>();
            GetChildCss(node, removed);
            while (removed.Count > 1)
            {
                ITvaModelNode n = removed.Pop();
                RemoveNodeCssWithRow(n);
            }
            RemoveNode(parent, node,false); //删除这个节点，孩子也一并删除了

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

        #region 移动节点
        /// <summary>
        /// 移动已知的node节点到另一个节点之下。
        /// </summary>
        /// <param name="fromPath">从哪里移动</param>
        /// <param name="node">被移动的节点</param>
        /// <param name="toPath">移动的目的地方</param>
        /// <param name="index">位于目的地方的同层孩子第几个。</param>
        /// <returns>返回最终被移动到新位置的节点</returns>
        public ITvaModelNode Move(TreePath fromPath, ITvaModelNode node, TreePath toPath, int index)
        {
            //先删除            
            return Add(RemoveNode(fromPath, node,true), toPath, index,true);

        }
        #endregion

        #region 完全更新ID并保存树
        /// <summary>
        /// 递归更新孩子的逻辑ID
        /// </summary>
        /// <param name="node"></param>
        internal void UpdateChildLogicId(ITvaModelNode node)
        {
            if (treeNodesMap.ContainsKey(node.Key.ToString()))//它有孩子
            {
                ITvaModelNode[] child = treeNodesMap[node.Key.ToString()];
                for (int i = 0; i < child.Length; i++)
                {
                    child[i].LogicKey = String.Format("{0}{1,3:D3}", node.LogicKey, i + 1);
                    //更新对应的行
                    dtTreeNodeMo.Select(String.Format("'{0}'={1}", child[i].Key, schema.ColumnName_Key))[0][schema.ColumnName_LogicKey] = child[i].LogicKey;
                    UpdateChildLogicId(child[i]);
                }
            }
        }

        /// <summary>
        /// 仅更新ID
        /// </summary>
        public void FullUpdateID()
        {
            if (allNodesMap.Count > 0)
            {
                allNodesMap.Clear();
                ITvaModelNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];                
       
                for (int i = 0; i < root.Length; i++) //更新根节点的逻辑Key
                {
                    root[i].LogicKey = String.Format("{0,3:D3}", i + 1);
                    DataRow row = dtTreeNodeMo.Select(String.Format("{1}='{0}'", root[i].Key, schema.ColumnName_Key))[0];
                    row[schema.ColumnName_LogicKey] = root[i].LogicKey;

                    allNodesMap.Add(root[i].LogicKey, root[i]);
                    UpdateChildLogicId(root[i]);//递归更新孩子结点的逻辑Key

                    if (AutoMapLogicKey2Key)
                    {
                        root[i].Key = root[i].LogicKey;//将逻辑Key同步到真Key
                        row[schema.ColumnName_Key] = root[i].Key;
                    }
                }
                if (AutoMapLogicKey2Key)
                {
                    foreach (DataRow row in dtTreeNodeMo.Rows)//对所有行进行处理
                    {
                        row[schema.ColumnName_Key] = row[schema.ColumnName_LogicKey];//逻辑ID列值赋给真正的ID列
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
                dtTreeNodeMo.AcceptChanges();//所有的逻辑ID更新完毕，且dt也全部同步，map中只有root节点    
            
                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }
        }

      
        /// <summary>
        /// 更新所有ID和逻辑ID并保存整个树到DB中。
        /// 会先清除旧的树，再添加当前树。
        /// </summary>
        public void FullUpdateIDAndSave()
        {
            if (allNodesMap.Count > 0)
            {
                allNodesMap.Clear();
                ITvaModelNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];
                //清除数据库中久的Key
                StringBuilder sbdel = new StringBuilder();
                sbdel.Append(" WHERE ");
                for (int i = 0; i < root.Length; i++)
                {
                    sbdel.AppendFormat(" {1} LIKE '{0}%' ", root[i].Key,schema.ColumnName_Key);
                    if (i < root.Length - 1) sbdel.Append(" OR ");
                }
                //用于删除旧的记录
                Trace.TraceInformation("删除给定根集合及其子节点:{0}", sbdel.ToString());
                for (int i = 0; i < root.Length; i++)
                {
                    root[i].LogicKey = String.Format("{0,3:D3}", i + 1);                  
                    allNodesMap.Add(root[i].Key.ToString(), root[i]);
                    UpdateChildLogicId(root[i]);//更新孩子结点的逻辑Key
                    if (AutoMapLogicKey2Key)
                    {
                        root[i].Key = root[i].LogicKey;//将逻辑Key同步到真Key                       
                    }
                    
                }

                if (AutoMapLogicKey2Key) //若进行了映射，则应同步更新父Key
                {
                    foreach (DataRow row in dtTreeNodeMo.Rows)
                    {
                        row[schema.ColumnName_Key] = row[schema.ColumnName_LogicKey];//逻辑ID列值赋给真正的ID列
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
                dtTreeNodeMo.AcceptChanges();//所有的逻辑ID更新完毕，且dt也全部同步，map中只有root节点    
                //将dt存储到表中
                try
                {
                    da.SaveTree(sbdel.ToString(), dtTreeNodeMo);
                }
                catch (DbException ex)
                {
                    Trace.Fail("保存树倒DB失败！", ex.Message);
                }
                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }

        }
        #endregion
        #endregion

        /// <summary>
        /// 同步DT到树
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
                    ITvaModelNode n = treePath.LastNode as ITvaModelNode;//获取它的孩子
                    if (treeNodesMap.ContainsKey((string)n.Key))
                    {
                        return treeNodesMap[(string)n.Key];
                    }                 
                    else
                    {
                        //找所有的直属孩子
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
