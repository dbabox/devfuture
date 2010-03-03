using System;
using System.Collections.Generic;
using System.Text;
using Aga.Controls.Tree;
using TreeEditor.Core;

using System.Data;
using System.Diagnostics;

namespace TreeEditor
{
    public class DataRowTvaNode
    {
        private DataRow row;

        public DataRow DataRow
        {
            get { return row; }
            set { row = value; }
        }
        private TvaSchema rowSchema;
        public DataRowTvaNode(DataRow row_, TvaSchema rowSchema_)
        {
            row = row_;
            rowSchema = rowSchema_;
       
        }

    


        #region ITvaNode 成员


       
        
        /// <summary>
        /// 赋值时连同row一起赋值更新了。
        /// 需坚持ID不变原则
        /// </summary>
        public string TNA_ID
        {
            get
            {
                return row[rowSchema.Tna_id_field_name].ToString();
            }
            set
            {
                
                row[rowSchema.Tna_id_field_name] = value;
            }
        }

        
        /// <summary>
        /// 父ID
        /// </summary>
        public string TNA_PID
        {
            get
            {
                return row[rowSchema.Tna_pid_field_name].ToString();
            }
            set
            {                
                row[rowSchema.Tna_pid_field_name] = value;
            }
        }

     
        public string TNA_Text
        {
            get
            {
                return row[rowSchema.Tna_text_field_name].ToString();
            }
            set
            {
               
                row[rowSchema.Tna_text_field_name] = value;
            }
        }

        private string tna_LogicId;
        /// <summary>
        /// 编辑器要使用的逻辑ID
        /// </summary>
        public string TNA_LogicId
        {
            get
            {
                return tna_LogicId;
            }
            set
            {
                tna_LogicId = value;
                //如果表具有逻辑ID字段，则自动映射上去
                if (!String.IsNullOrEmpty(rowSchema.Tna_logic_id_map_field))
                {
                    row[rowSchema.Tna_logic_id_map_field] = value;
                }
            }
        }

      

        protected System.Drawing.Image icon;
        /// <summary>
        /// 图标
        /// </summary>
        public virtual System.Drawing.Image Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
            }
        }

        private bool isChecked;
        /// <summary>
        /// 多选时是否选中
        /// </summary>
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

        #region ICloneable 成员

        public object Clone()
        {
            DataRow newrow = row.Table.NewRow();
            row.ItemArray.CopyTo(newrow.ItemArray, 0);
            return new DataRowTvaNode(newrow, this.rowSchema);

        }

        #endregion

       

        public static DataRowTvaNode[] CreateNodes(TvaSchema rowSchema, params DataRow[] rows)
        {
            DataRowTvaNode[] nodes = new DataRowTvaNode[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                nodes[i] = new DataRowTvaNode(rows[i], rowSchema);
            }
            return nodes;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (i < row.Table.Columns.Count - 1)
                {
                    sb.AppendFormat("{0}={1}/", row.Table.Columns[i].ColumnName, row[i]);
                }
                else
                {
                    sb.AppendFormat("{0}={1}", row.Table.Columns[i].ColumnName, row[i]);
                }
            }
            return sb.ToString();
        }

    }

    public class DataRowTreeModel : TreeModelBase
    {
        private DataTableTreeAdapter tta;

        public DataTableTreeAdapter Tta
        {
            get { return tta; }
            set { tta = value; }
        }

        

        #region 从配置文件获取
        /// <summary>
        /// 根的逻辑ID格式 [前缀][###]
        /// </summary>
        string LOGIC_ID_FMT_1 = "{1}{0,3:D3}";
        /// <summary>
        /// 支及叶子的逻辑ID格式
        /// </summary>
        string LOGIC_ID_FMT_2 = "{0,3:D3}{1,3:D3}";
        /// <summary>
        /// 根逻辑ID前缀
        /// </summary>
        string ROOT_LOGIC_ID_PREFIX = String.Empty;
        /// <summary>
        /// 仅使用指定的单根ID
        /// </summary>
        bool USE_SINGLE_SPECIAL_ROOT_ID = false;
        /// <summary>
        /// 给定特定的单根ID
        /// </summary>
        string SPECIALED_SIGNLE_ROOT_ID = String.Empty;

        const string Default_TVA_ROOT_KEY = "ROOT";

        bool is_auto_backup = true;

        string backup_path = System.IO.Path.Combine(Environment.CurrentDirectory, "backup");

        #endregion



        private int nodesTotalCount;



 

        private Dictionary<string, DataRowTvaNode> allNodesMap;
        /// <summary>
        /// 所有节点。当且仅当树调用ExpandAll之后，它才真正包含所有节点；
        /// </summary>
        internal Dictionary<string, DataRowTvaNode> AllNodesMap
        {
            get { return allNodesMap; }

        }
             
        //必须实现节点本地CRUD       
        /// <summary>
        /// 父节点ID，邻接孩子列表，这里父节点ID，不能为空。
        /// </summary>
        private  Dictionary<string, DataRowTvaNode[]> treeNodesMap = new Dictionary<string, DataRowTvaNode[]>();

        public Dictionary<string, DataRowTvaNode[]> TreeNodesMap
        {
            get { return treeNodesMap; }
        }



        //非随需加载时，使用此变量保存所有节点的表数据
        private DataTable dtTreeNodeMo;

        public DataTable DtTreeNodeMo
        {
            get { return dtTreeNodeMo; }
        }

        public DataRowTreeModel(TvaSchema schema_)
        {
            tta = new DataTableTreeAdapter(schema_);
            #region 配置信息
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_1"]))
            {
                LOGIC_ID_FMT_1 = System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_1"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_2"]))
            {
                LOGIC_ID_FMT_2 = System.Configuration.ConfigurationManager.AppSettings["LOGIC_ID_FMT_2"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["ROOT_LOGIC_ID_PREFIX"]))
            {
                ROOT_LOGIC_ID_PREFIX = System.Configuration.ConfigurationManager.AppSettings["ROOT_LOGIC_ID_PREFIX"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SIGNLE_ROOT_ID"]))
            {
                SPECIALED_SIGNLE_ROOT_ID = System.Configuration.ConfigurationManager.AppSettings["SIGNLE_ROOT_ID"];
            }
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"]))
            {
                string boolstr = System.Configuration.ConfigurationManager.AppSettings["USE_SINGLE_ROOT_ID"];
                if (bool.TryParse(boolstr, out USE_SINGLE_SPECIAL_ROOT_ID) == false)
                {
                    int intvalue = 0;
                    if (Int32.TryParse(boolstr, out intvalue) == true)
                    {
                        USE_SINGLE_SPECIAL_ROOT_ID = intvalue != 0;
                    }
                }
                else
                {
                    USE_SINGLE_SPECIAL_ROOT_ID = false;
                }
            }

            if (System.Configuration.ConfigurationManager.AppSettings["AutoBackup"] != null)
            {
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["AutoBackup"], out is_auto_backup);
            }
            #endregion

            dtTreeNodeMo = tta.GetTreeNodeDataTable().Tables[0];
            InitTreeModelViaDataTable();
        }

        public DataRowTreeModel():this(new TvaSchema())
        {

        }

        
        /// <summary>
        /// 从指定的根加载树 , 考虑通用实现，这里要求树的源表警告TreeEditor的标准化处理，
        /// 包括逻辑层次ID字段，形如：001001002... 有时候，逻辑层次ID字段，就是主键字段。
        /// 但不推荐这么做，因为主键一旦成为外键，不可更改。
        /// </summary>
        /// <param name="rootFilter"></param>
        private void InitTreeModelViaDataTable()
        {
            DataRow[] rootRows = dtTreeNodeMo.Select( tta.Schema.RowFilter_GetRootNodes );//默认根为类型码1代表根

            treeNodesMap = new Dictionary<string, DataRowTvaNode[]>();
            allNodesMap = new Dictionary<string, DataRowTvaNode>(dtTreeNodeMo.Rows.Count);

            DataRowTvaNode[] roots = new DataRowTvaNode[rootRows.Length];
            for (int i = 0; i < rootRows.Length; i++)
            {
                roots[i] = new DataRowTvaNode(rootRows[i],tta.Schema);
                allNodesMap.Add(roots[i].TNA_ID, roots[i]);
                Trace.TraceInformation("初始化根节点{0}.", roots[i]);
            }
            lock (treeNodesMap)
            {
                treeNodesMap.Add(Default_TVA_ROOT_KEY, roots);
            }
                   
           
        }


        #region 从DB加载树
        /// <summary>
        /// 使用适配器刷新Model.
        /// </summary>
        public void RefreshFromAdapter()
        {
            Trace.WriteLine("从DB加载树.");
            nodesTotalCount = tta.GetNodesTotalCount();
            if (allNodesMap == null)
            {
                allNodesMap = new Dictionary<string, DataRowTvaNode>(nodesTotalCount);
            }
            else
            {
                allNodesMap.Clear();
            }

            treeNodesMap.Clear();
            DataSet treeDs = tta.GetTreeNodeDataTable();

            #region //这里自动备份
            if (is_auto_backup)
            {
                if (!System.IO.Directory.Exists(backup_path))
                {
                    System.IO.Directory.CreateDirectory(backup_path);
                }
                string backupFileName = System.IO.Path.Combine(backup_path, Guid.NewGuid().ToString("D") + ".xml");
                treeDs.WriteXml(backupFileName, XmlWriteMode.WriteSchema);
                Trace.TraceInformation("备份{0}到{1}", tta.GetType(), backupFileName);
            }
            #endregion
            dtTreeNodeMo = treeDs.Tables[0];
            nodesTotalCount = dtTreeNodeMo.Rows.Count;            
            base.Refresh();
        }

        #endregion

        #region 必须重写的函数
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
                    DataRowTvaNode n = treePath.LastNode as DataRowTvaNode;//获取它的孩子
                    if (treeNodesMap.ContainsKey(n.TNA_ID))
                    {
                        return treeNodesMap[n.TNA_ID];
                    }
                    else
                    {
                        //找所有的直属孩子
                        DataRow[] rows = dtTreeNodeMo.Select(String.Format("{0}='{1}'",
                            tta.Schema.Tna_pid_field_name , n.TNA_ID));
                        if (rows != null && rows.Length > 0)
                        {
                            DataRowTvaNode[] nodeArray = new DataRowTvaNode[rows.Length];
                            for (int r = 0; r < rows.Length; r++)
                            {
                                nodeArray[r] = new DataRowTvaNode(rows[r], tta.Schema);
                                allNodesMap.Add(nodeArray[r].TNA_ID, nodeArray[r]);
                            }
                            lock (treeNodesMap)
                            {
                                treeNodesMap.Add(n.TNA_ID, nodeArray);
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
            DataRowTvaNode n = treePath.LastNode as DataRowTvaNode;
            if (n == null) return true;
            return Convert.ToInt32(dtTreeNodeMo.Compute(String.Format("COUNT({0})", tta.Schema.Tna_id_field_name),
                String.Format("{0}='{1}'",tta.Schema.Tna_pid_field_name, n.TNA_ID))) == 0;
        }
        #endregion

        #region 表格数据同步到树，这时可以实现添加。
        /// <summary>
        /// 表格数据同步到树，这时可以实现添加。
        /// </summary>
        /// <returns></returns>
        public int SyncDataTable2TreeNodes()
        {
            dtTreeNodeMo.AcceptChanges();         
            this.nodesTotalCount = dtTreeNodeMo.Rows.Count;
            treeNodesMap.Clear();
          
            allNodesMap.Clear();
           
            //重新构建树
            Refresh();//这是个长时间的过程。
            return allNodesMap.Count;
        }
        #endregion

        #region 所有TVA树可共用的方法



        #region 添加新节点到树

        /// <summary>
        /// 添加新节点到某个指定父亲的某个指定位置。
        /// </summary>
        /// <param name="node">必须给定Key和文本。</param>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataRowTvaNode Add(DataRowTvaNode node, TreePath parent, int index)
        {
            //父一定要改变
            node.TNA_PID = (parent == TreePath.Empty) ? null : ((DataRowTvaNode)parent.LastNode).TNA_ID;
            //确定父亲键
            string parentId = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((DataRowTvaNode)parent.LastNode).TNA_ID;

            DataRowTvaNode[] newArr = null;
            if (treeNodesMap.ContainsKey(parentId))
            {
                //原数组长度
                DataRowTvaNode[] oldArr = treeNodesMap[parentId];
                int oldLenth = oldArr.Length;
                newArr = new DataRowTvaNode[oldLenth + 1];
                Array.Copy(oldArr, 0, newArr, 0, index);
                newArr[index] = node;
                Trace.TraceInformation("为节点赋予了完整的ParentKey和临时唯一Key:{0}", node);
                if (newArr.Length > (index + 1)) //还有没copy的
                {
                    Array.Copy(oldArr, index, newArr, index + 1, oldArr.Length - index);
                }

            }
            else
            {
                newArr = new DataRowTvaNode[] { node };
            }
            treeNodesMap[parentId] = newArr; //赋予了新的数组
            allNodesMap.Add(node.TNA_ID, node);//此时Key是GUID，不可能重复
            dtTreeNodeMo.Rows.Add(node.DataRow);
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
        internal DataRowTvaNode RemoveNode(TreePath parent, DataRowTvaNode node)
        {
            //确定父亲键
            string partentKey = (parent == TreePath.Empty) ? Default_TVA_ROOT_KEY :
                ((DataRowTvaNode)parent.LastNode).TNA_ID;
            DataRowTvaNode[] oldArr = treeNodesMap[partentKey];
            if (oldArr.Length == 1) //只有1个孩子，那么肯定是node
            {
                System.Diagnostics.Trace.Assert(node.TNA_ID == oldArr[0].TNA_ID);
                Trace.TraceInformation("父{0}下只有1个孩子，且删除它:{1}", partentKey, node);
                //执行删除
                treeNodesMap.Remove(partentKey);
            }
            else
            {
                DataRowTvaNode[] newArr = new DataRowTvaNode[oldArr.Length - 1];
                int removedNodeIndex = -1;
                for (int i = 0; i < oldArr.Length; i++)
                {
                    if (oldArr[i] == node || oldArr[i].TNA_ID == node.TNA_ID)
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
            allNodesMap.Remove((string)node.TNA_ID);
            DataRow[] rows = dtTreeNodeMo.Select(String.Format("{1}='{0}'", node.TNA_ID, tta.Schema.Tna_id_field_name));
            if (rows.Length == 1) dtTreeNodeMo.Rows.Remove(rows[0]);
            OnNodesRemoved(new TreeModelEventArgs(parent, new object[] { node }));
            return node;
        }
        /// <summary>
        /// 级联删除时使用，将一个节点彻底删除
        /// </summary>
        /// <param name="node"></param>
        internal void RemoveNodeCssWithRow(DataRowTvaNode node)
        {
            if (treeNodesMap.ContainsKey((string)node.TNA_ID))
            {
                treeNodesMap.Remove((string)node.TNA_ID);
            }
            allNodesMap.Remove((string)node.TNA_ID);
            DataRow[] rows = dtTreeNodeMo.Select(String.Format("{1}='{0}'", node.TNA_ID, tta.Schema.Tna_id_field_name));
            if (rows.Length == 1) dtTreeNodeMo.Rows.Remove(rows[0]);
        }

        /// <summary>
        /// 删除现存的一个节点。删除实际是从孩子集合中移除。
        /// </summary>
        /// <param name="parent">父节点路径.</param>
        /// <param name="node">被删除的节点，只能是叶子。</param>
        public void Remove(TreePath parent, DataRowTvaNode node)
        {
            Stack<DataRowTvaNode> removed = new Stack<DataRowTvaNode>();
            GetChildCss(node, removed);
            while (removed.Count > 1)
            {
                DataRowTvaNode n = removed.Pop();
                RemoveNodeCssWithRow(n);
            }
            RemoveNode(parent, node); //删除这个节点，孩子也一并删除了

        }





        internal void GetChildCss(DataRowTvaNode node, Stack<DataRowTvaNode> stack)
        {
            stack.Push(node);
            if (treeNodesMap.ContainsKey((string)node.TNA_ID))
            {
                foreach (DataRowTvaNode n in treeNodesMap[(string)node.TNA_ID])
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
        public DataRowTvaNode Move(TreePath fromPath, DataRowTvaNode node, TreePath toPath, int index)
        {
            //先删除            
            return Add(RemoveNode(fromPath, node), toPath, index);

        }
        #endregion

        #region 完全更新ID并保存树
        /// <summary>
        /// 递归更新孩子的逻辑ID
        /// </summary>
        /// <param name="node"></param>
        internal void UpdateChildLogicId(DataRowTvaNode node)
        {
            if (treeNodesMap.ContainsKey(node.TNA_ID))//它有孩子
            {
                DataRowTvaNode[] child = treeNodesMap[node.TNA_ID];
                for (int i = 0; i < child.Length; i++)
                {
                    child[i].TNA_LogicId = String.Format("{0}{1,3:D3}", node.TNA_LogicId, i + 1);
                    dtTreeNodeMo.Select(String.Format("{1}='{0}'", child[i].TNA_ID, tta.Schema.Tna_id_field_name))[0][tta.Schema.Tna_logic_id_map_field] = child[i].TNA_LogicId;
                    UpdateChildLogicId(child[i]);
                }
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
                DataRowTvaNode[] root = treeNodesMap[Default_TVA_ROOT_KEY];
                //清除数据库中久的Key
                StringBuilder sbdel = new StringBuilder();
                sbdel.Append(" WHERE ");
                for (int i = 0; i < root.Length; i++)
                {
                    sbdel.AppendFormat(" {1} LIKE '{0}%' ", root[i].TNA_ID, tta.Schema.Tna_id_field_name);
                    if (i < root.Length - 1) sbdel.Append(" OR ");
                }
                //用于删除旧的记录
                Trace.TraceInformation("删除给定根集合及其子节点:{0}", sbdel.ToString());
                for (int i = 0; i < root.Length; i++)
                {
                    root[i].TNA_LogicId = String.Format("{0,3:D3}", i + 1);
                    DataRow row = dtTreeNodeMo.Select(String.Format("{1}='{0}'", root[i].TNA_ID, tta.Schema.Tna_id_field_name))[0];
                    row[tta.Schema.Tna_logic_id_map_field] = root[i].TNA_LogicId;

                    allNodesMap.Add(root[i].TNA_LogicId, root[i]);
                    UpdateChildLogicId(root[i]);//更新孩子结点的逻辑Key
                    root[i].TNA_ID = root[i].TNA_LogicId;//将逻辑Key同步到真Key
                    row[tta.Schema.Tna_id_field_name] = root[i].TNA_ID;
                }
                foreach (DataRow row in dtTreeNodeMo.Rows)
                {
                    row[tta.Schema.Tna_id_field_name] = row[tta.Schema.Tna_logic_id_map_field];//逻辑ID列值赋给真正的ID列
                    string lkey = row[tta.Schema.Tna_logic_id_map_field].ToString();
                    if (lkey.Length > 3)
                    {
                        row[tta.Schema.Tna_pid_field_name] = lkey.Substring(0, lkey.Length - 3);
                    }
                    else
                    {
                        row[tta.Schema.Tna_pid_field_name] = null;
                    }
                }
                dtTreeNodeMo.AcceptChanges();//所有的逻辑ID更新完毕，且dt也全部同步，map中只有root节点    
                //将dt存储到表中
                //da.SaveTree(sbdel.ToString(), dtTreeNodeMo);
                //TODO:真正保存到DB

                treeNodesMap.Clear();
                treeNodesMap.Add(Default_TVA_ROOT_KEY, root);
                OnStructureChanged(new TreePathEventArgs(TreePath.Empty));

            }

        }
        #endregion
        #endregion
         
    }
}
